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
