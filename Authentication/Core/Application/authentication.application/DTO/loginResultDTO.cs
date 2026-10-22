namespace authentication.application.DTO
{
    /// <summary>Outcome of a login attempt. Token is only set when Success is true.</summary>
    public class loginResultDTO
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Error { get; set; }

        public static loginResultDTO Ok(string token) => new() { Success = true, Token = token };
        public static loginResultDTO Fail(string error) => new() { Success = false, Error = error };
    }
}
