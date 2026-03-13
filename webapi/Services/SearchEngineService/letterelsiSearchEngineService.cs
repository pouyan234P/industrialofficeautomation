using webapi.Model;
using webapi.Model.Searchengine;
using webapi.Services.IServices.ISearchEngineService;

namespace webapi.Services.SearchEngineService
{
    public class letterelsiSearchEngineService : BaseService, IletterelsiSearchEngineService
    {
        private readonly IHttpClientFactory _clientFactory;

        public letterelsiSearchEngineService(IHttpClientFactory clientFactory):base(clientFactory) 
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> search<T>(SearchRequestDto searchRequest)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data=searchRequest,
                Url=SD.gatewayApiBase+ "/api/letterelsi/search"
            });
        }
    }
}
