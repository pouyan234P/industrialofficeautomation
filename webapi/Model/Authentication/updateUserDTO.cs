namespace webapi.Model.Authentication
{
    /// <summary>
    /// Body of PUT api/user/updateUser/{id}, sent by the Angular "Edit User" page.
    /// password and signitureImageId are optional: null keeps the current value.
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
