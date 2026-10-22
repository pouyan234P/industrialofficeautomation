namespace webapi.Model.Authentication
{
    /// <summary>Body of POST api/role/create, sent by the Angular "New Role" form.</summary>
    public class createRoleDTO
    {
        public string? name { get; set; }
    }
}
