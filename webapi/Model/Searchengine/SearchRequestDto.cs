namespace webapi.Model.Searchengine
{
    public class SearchRequestDto
    {

        public string? keyword { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}
