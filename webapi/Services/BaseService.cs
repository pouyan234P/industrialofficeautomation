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
        public ResponseDTO responseDTO { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<(byte[] Data, string ContentType)> SendFileAsync(ApiRequest apiRequest)
        {
            var client = httpClient.CreateClient("industrial");
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(apiRequest.Url),
                Method = HttpMethod.Get
            };

            if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);

            var apiResponse = await client.SendAsync(message);

            if (!apiResponse.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch file. Status: {apiResponse.StatusCode}");

            var bytes = await apiResponse.Content.ReadAsByteArrayAsync();
            var contentType = apiResponse.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

            return (bytes, contentType);
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

                if (apiRequest.ContentType == "multipart/form-data" && apiRequest.Data != null)
                {
                    var content = new MultipartFormDataContent();
                    foreach (var prop in apiRequest.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(apiRequest.Data);
                        if (value != null)
                        {
                            if (value is byte[] fileBytes)
                            {
                                var fileContent = new ByteArrayContent(fileBytes);
                                content.Add(fileContent, prop.Name, "upload.jpg");
                            }
                            else
                            {
                                content.Add(new StringContent(value.ToString()), prop.Name);
                            }
                        }
                    }
                    message.Content = content;
                }
                else if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(
                        JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,
                        "application/json");
                }

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);

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

                var apiresponse = await client.SendAsync(message);
                var apicontent = await apiresponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(apicontent);
            }
            catch (Exception e)
            {
                var dto = new ResponseDTO
                {
                    DisplayMessage = "ERROR",
                    ErrorMessages = new List<string> { e.Message },
                    IsSuccess = false
                };
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dto));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}