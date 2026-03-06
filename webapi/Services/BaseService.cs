using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using webapi.Model;
using webapi.Services.IServices;

namespace webapi.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory httpClient;

        public ResponseDTO responseDTO { get ; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("industrial");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                var stringContent = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                message.Content = stringContent;
                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer ", apiRequest.AccessToken);
                }
                HttpResponseMessage apiresponse = null;
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                apiresponse = await client.SendAsync(message);
                var apicontent = await apiresponse.Content.ReadAsStringAsync();
                var apiresponsedto = JsonConvert.DeserializeObject<T>(apicontent);
                
                return apiresponsedto;
            }
            catch (Exception e)
            {
                var dto = new ResponseDTO
                {
                    DisplayMessage = "ERROR",
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiresponsedto = JsonConvert.DeserializeObject<T>(res);
                return apiresponsedto;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
