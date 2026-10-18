namespace authentication.application.DTO
{
    /// <summary>Body of PUT api/auth/users/{id}/roles — the complete list of roles the user should hold.</summary>
    public class setUserRolesDTO
    {
        public List<string>? roles { get; set; }
    }
}
