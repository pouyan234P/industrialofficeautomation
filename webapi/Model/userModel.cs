using webapi.Model.Authentication;

namespace webapi.Model
{
    public class userModel
    {
        public string username { get; set; }
        public int personID { get; set; }
        public bool CurrentStatus { get; set; }
        public string? signitureimageid { get; set; }
        public string? Title { get; set; }
    }
}
