using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Options;
using searchengine.Application.IRepository;
using searchengine.domain;
using searchengine.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchengine.Persistence.Repository
{
    public class LetterSearchRepository : ILetterSearchRepository
    {
        private readonly ElasticsearchClient _client;

        public LetterSearchRepository(ElasticsearchClient client)
        {
            _client = client;
        }
        public async Task<bool> IndexLetterAsync(LetterSearchDocument document)
        {
            var response = await _client.IndexAsync(document, idx => idx
        .Index("letters_index")
        .Id(document.LetterId.ToString())
    );

            if (!response.IsValidResponse)
            {
                // Put a breakpoint here or log these details:

                // 1. Gives you the raw HTTP request/response and exact error
                var debugInfo = response.DebugInformation;

                // 2. Gives you the specific error from the Elasticsearch server
                var serverError = response.ElasticsearchServerError; // Note: Use response.ServerError if using v7 NEST

                // 3. Catches underlying network/connection exceptions
                // Fix for CS7036: Provide the required 'out' parameter for TryGetOriginalException
                if (response.TryGetOriginalException(out var exception))
                {
                    // Log or handle the exception as needed
                    Console.WriteLine($"Debug Info: {exception.Message}");
                };

                // Example logging:
                Console.WriteLine($"Debug Info: {debugInfo}");
            }

            return response.IsValidResponse;
        }

        [Obsolete]
        public async Task<IEnumerable<LetterSearchDocument>> SearchAsync(string keyword, DateTime? fromDate, DateTime? toDate)
        {
            var response = await _client.SearchAsync<LetterSearchDocument>(s => s
        .Index("letters_index")
        .Size(50)
        .Query(q => q
            .Bool(b =>
            {
                // 1. اضافه کردن شرط Must (جستجوی متنی)
                // کلمه کلیدی return را حذف کردیم و مستقیماً روی b صدا می‌زنیم
                b.Must(m => m
                    .MultiMatch(mm => mm
                        .Query(keyword)
                        .Fields(new[] { "subject^3", "plainTextBody", "ocrContent", "letterNo^2" })
                    )
                );

                // 2. اضافه کردن شرط Filter (فقط در صورتی که تاریخ ارسال شده باشد)
                // با این روش، اگر تاریخی نباشد اصلا بلاک Filter به کوئری اضافه نمی‌شود که بسیار بهینه‌تر است
                if (fromDate.HasValue || toDate.HasValue)
                {
                    b.Filter(f => f
                        .Range(r => r
                            .DateRange(dr => dr
                                .Field(field => field.CreatedDate)
                                .Gte(fromDate) // بزرگتر مساوی
                                .Lte(toDate)   // کوچکتر مساوی
                            )
                        )
                    );
                }
            })
        )
    );

            if (!response.IsValidResponse)
            {
                // پیشنهاد: اینجا response.DebugInformation را لاگ کن تا اگر اروری بود بتوانی ببینی
                return new List<LetterSearchDocument>();
            }

            return response.Documents.ToList();
        }
    }
}
