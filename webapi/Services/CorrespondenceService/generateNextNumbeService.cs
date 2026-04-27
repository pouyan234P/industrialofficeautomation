using webapi.Model;
using webapi.Model.CorrespondenceModel;
using webapi.Model.CorrespondenceModel.Enum;
using webapi.Services.IServices.ICorrespondenceService;

namespace webapi.Services.CorrespondenceService
{
    public class generateNextNumbeService : BaseService, IgenerateNextNumbeService
    {
        private readonly IHttpClientFactory  _clientFactory;

        public generateNextNumbeService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor) : base(clientFactory,httpContextAccessor)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> getlastNumberbyType<T>(TypeDTO type, int depid, int year)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.GET,
                Url=SD.gatewayApiBase+ "/api/generateNextNumber/getlastNumberbyType/" + type+"/"+depid+"/"+year
            });
        }

        public async Task<T> mynextnumber<T>(getNextNumberDTO dto)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.GET,
                Data=dto,
                Url=SD.gatewayApiBase+ "/api/generateNextNumber/getnextnumber"
            });
        }
    }
}
