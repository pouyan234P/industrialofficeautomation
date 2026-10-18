namespace authentication.application.DTO
{
    /// <summary>A role as shown on the Roles page.</summary>
    public class roleDTO
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        /// <summary>How many users currently hold this role.</summary>
        public int userCount { get; set; }
    }
}
