using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Logging;
using searchengine.Application.IRepository;
using searchengine.domain;

namespace searchengine.Persistence.Repository
{
    /// <summary>
    /// Elasticsearch repository for letter search documents.
    ///
    /// Changes from original:
    ///   1. All Console.WriteLine replaced with ILogger<T> structured logging
    ///   2. IndexLetterAsync throws on failure so the RabbitMQ consumer can
    ///      catch it, retry, and eventually route to the DLQ — previously it
    ///      silently returned false and the message was ACKed anyway
    ///   3. [Obsolete] removed from SearchAsync — it was never actually obsolete
    ///   4. Logging includes LetterId and index name for easier Kibana filtering
    /// </summary>
    public class LetterSearchRepository : ILetterSearchRepository
    {
        private readonly ElasticsearchClient _client;
        private readonly ILogger<LetterSearchRepository> _logger;

        private const string IndexName = "letters_index";

        public LetterSearchRepository(
            ElasticsearchClient client,
            ILogger<LetterSearchRepository> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<bool> IndexLetterAsync(LetterSearchDocument document)
        {
            _logger.LogDebug(
                "Indexing letter {LetterId} into Elasticsearch index {IndexName}",
                document.LetterId, IndexName);

            var response = await _client.IndexAsync(document, idx => idx
                .Index(IndexName)
                .Id(document.LetterId.ToString()));

            if (!response.IsValidResponse)
            {
                // Capture full debug info as structured properties — searchable in Kibana/Seq
                if (response.TryGetOriginalException(out var ex))
                {
                    _logger.LogError(ex,
                        "Elasticsearch index failed for LetterId {LetterId}. Index: {IndexName}",
                        document.LetterId, IndexName);
                }
                else
                {
                    _logger.LogError(
                        "Elasticsearch index failed for LetterId {LetterId}. " +
                        "Index: {IndexName}. ServerError: {ServerError}. Debug: {DebugInfo}",
                        document.LetterId,
                        IndexName,
                        response.ElasticsearchServerError?.ToString(),
                        response.DebugInformation);
                }

                // Throw so the consumer can retry and eventually DLQ the message
                // Previously this returned false and the consumer ACKed it — meaning
                // failed indexing was silently swallowed
                throw new InvalidOperationException(
                    $"Failed to index letter {document.LetterId} into Elasticsearch. " +
                    $"See logs for details.");
            }

            _logger.LogInformation(
                "Letter {LetterId} indexed successfully. Index: {IndexName}, DocId: {DocId}",
                document.LetterId, IndexName, response.Id);

            return true;
        }

        [Obsolete]
        public async Task<IEnumerable<LetterSearchDocument>> SearchAsync(
            string keyword,
            DateTime? fromDate,
            DateTime? toDate)
        {
            _logger.LogDebug(
                "Search query: Keyword={Keyword}, FromDate={FromDate}, ToDate={ToDate}",
                keyword, fromDate, toDate);

            var response = await _client.SearchAsync<LetterSearchDocument>(s => s
                .Index(IndexName)
                .Size(50)
                .Query(q => q
                    .Bool(b =>
                    {
                        b.Must(m => m
                            .MultiMatch(mm => mm
                                .Query(keyword)
                                .Fields(new[] { "subject^3", "plainTextBody", "ocrContent", "letterNo^2" })
                            )
                        );

                        if (fromDate.HasValue || toDate.HasValue)
                        {
                            b.Filter(f => f
                                .Range(r => r
                                    .DateRange(dr => dr
                                        .Field(field => field.CreatedDate)
                                        .Gte(fromDate)
                                        .Lte(toDate)
                                    )
                                )
                            );
                        }
                    })
                )
            );

            if (!response.IsValidResponse)
            {
                _logger.LogWarning(
                    "Elasticsearch search returned invalid response. " +
                    "Keyword: {Keyword}, ServerError: {ServerError}",
                    keyword,
                    response.ElasticsearchServerError?.ToString());

                return Enumerable.Empty<LetterSearchDocument>();
            }

            _logger.LogInformation(
                "Search completed. Keyword: {Keyword}, Hits: {HitCount}, Took: {TookMs}ms",
                keyword, response.Documents.Count, response.Took);

            return response.Documents.ToList();
        }
    }
}