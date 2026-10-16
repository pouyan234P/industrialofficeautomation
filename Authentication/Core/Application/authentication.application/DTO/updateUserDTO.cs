namespace authentication.application.DTO
{
    /// <summary>
    /// Data sent by the "Edit User" page.
    /// Password and signitureImageId are optional: leave them null to keep the current values.
    /// </summary>
    public class updateUserDTO
    {
        public string? username { get; set; }
        public int personID { get; set; }
        public bool CurrentStatus { get; set; }
        public string? password { get; set; }
        public int? signitureImageId { get; set; }
    }
}
