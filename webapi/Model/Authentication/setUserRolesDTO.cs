namespace webapi.Model.Authentication
{
    /// <summary>Body of PUT api/user/{id}/roles — the full list of role names the user should hold.</summary>
    public class setUserRolesDTO
    {
        public List<string>? roles { get; set; }
    }
}
