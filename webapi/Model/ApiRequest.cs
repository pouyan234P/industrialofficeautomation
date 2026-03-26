using static webapi.SD;

namespace webapi.Model
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; }
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
        public string ContentType { get; set; } = "application/json";
    }
}
