using Newtonsoft.Json;
using Shared.Infrastructure.Middleware;
using System.Net.Http.Headers;
using System.Text;
using webapi.Model;
using webapi.Services.IServices;

namespace webapi.Services
{
    /// <summary>
    /// Base HTTP service for all downstream calls.
    ///
    /// Changes from original:
    ///   1. Forwards X-Correlation-Id on every outbound HTTP request
    ///      so downstream services (Correspondence, Workflow, Search) log
    ///      the same CorrelationId as the originating WebAPI request
    ///   2. IHttpContextAccessor injected to read the current CorrelationId
    ///      from the ambient request context
    /// </summary>
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResponseDTO responseDTO { get; set; }

        public BaseService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(byte[] Data, string ContentType)> SendFileAsync(ApiRequest apiRequest)
        {
            var client = _httpClient.CreateClient("industrial");
            var message = new HttpRequestMessage
            {
                RequestUri = new Uri(apiRequest.Url),
                Method = HttpMethod.Get
            };

            ForwardCorrelationId(message);
            SetAuthorization(message, apiRequest);

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
                var client = _httpClient.CreateClient("industrial");
                var message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();

                // ── Forward CorrelationId to every downstream service ────────
                // This is what links log lines across WebAPI → Correspondence →
                // Workflow → Search into one traceable chain
                ForwardCorrelationId(message);

                if (apiRequest.ContentType == "multipart/form-data" && apiRequest.Data != null)
                {
                    var content = new MultipartFormDataContent();
                    foreach (var prop in apiRequest.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(apiRequest.Data);
                        if (value != null)
                        {
                            if (value is IFormFile formFile)
                            {
                                var streamContent = new StreamContent(formFile.OpenReadStream());
                                streamContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                                content.Add(streamContent, prop.Name, formFile.FileName);
                            }
                            else if (value is byte[] fileBytes)
                            {
                                var fileContent = new ByteArrayContent(fileBytes);
                                content.Add(fileContent, prop.Name, "upload.jpg");
                            }
                            else
                            {
                                content.Add(new StringContent(value.ToString()!), prop.Name);
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

                SetAuthorization(message, apiRequest);

                message.Method = apiRequest.ApiType switch
                {
                    SD.ApiType.POST => HttpMethod.Post,
                    SD.ApiType.PUT => HttpMethod.Put,
                    SD.ApiType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get
                };

                var apiresponse = await client.SendAsync(message);
                var apicontent = await apiresponse.Content.ReadAsStringAsync();

                // 401/403 and some errors come back with an empty or non-JSON body;
                // turn those into a normal failed ResponseDTO instead of a null.
                T? parsed = default;
                if (!string.IsNullOrWhiteSpace(apicontent))
                {
                    try { parsed = JsonConvert.DeserializeObject<T>(apicontent); }
                    catch (JsonException) { parsed = default; }
                }
                if (parsed != null)
                    return parsed;

                Serilog.Log.Warning("Downstream call {Method} {Url} returned {Status} without a usable body",
                    message.Method, apiRequest.Url, (int)apiresponse.StatusCode);
                return Failure<T>(apiresponse.StatusCode switch
                {
                    System.Net.HttpStatusCode.Unauthorized => "You are not signed in or your session has expired.",
                    System.Net.HttpStatusCode.Forbidden => "You do not have permission to do this.",
                    _ => $"The service responded with {(int)apiresponse.StatusCode}."
                });
            }
            catch (Exception e)
            {
                // Log the details; don't send internal hosts/ports/stack traces to the browser.
                Serilog.Log.Error(e, "Downstream call {Method} {Url} failed", apiRequest.ApiType, apiRequest.Url);
                return Failure<T>("A backend service is unavailable. Please try again shortly.");
            }
        }

        private static T Failure<T>(string message)
        {
            var dto = new ResponseDTO
            {
                DisplayMessage = "ERROR",
                ErrorMessages = new List<string> { message },
                IsSuccess = false
            };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dto))!;
        }

        /// <summary>
        /// Uses the explicit token if one was given, otherwise forwards the caller's own
        /// "Authorization: Bearer ..." header so downstream services can authorize the same user.
        /// Set on the message (not the client) so it can never leak into another request.
        /// </summary>
        private void SetAuthorization(HttpRequestMessage message, ApiRequest apiRequest)
        {
            if (!string.IsNullOrEmpty(apiRequest.AccessToken))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                return;
            }

            var incoming = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            if (!string.IsNullOrEmpty(incoming) && AuthenticationHeaderValue.TryParse(incoming, out var header))
                message.Headers.Authorization = header;
        }

        /// <summary>
        /// Reads the CorrelationId that CorrelationIdMiddleware stored in HttpContext.Items
        /// and adds it as a request header so the downstream service receives it.
        /// </summary>
        private void ForwardCorrelationId(HttpRequestMessage message)
        {
            var correlationId = _httpContextAccessor.HttpContext?
                                    .Items[CorrelationIdMiddleware.HeaderName]?.ToString();

            if (!string.IsNullOrEmpty(correlationId))
                message.Headers.TryAddWithoutValidation(
                    CorrelationIdMiddleware.HeaderName, correlationId);
        }

        public void Dispose() => GC.SuppressFinalize(true);
    }
}