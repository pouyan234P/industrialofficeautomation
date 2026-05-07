namespace workflow.Api.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public string DisplayMessage { get; set; } = "";
        public List<string> ErrorMessages { get; set; }
        public int? currentPage { get; set; }
        public int? itemsPerPage { get; set; }
        public int? totalItems { get; set; }
        public int? totalPages { get; set; }
    }
}
