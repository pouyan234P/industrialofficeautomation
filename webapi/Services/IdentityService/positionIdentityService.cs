using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Services.IdentityService
{
    public class positionIdentityService : BaseService, IpositionIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;

        public positionIdentityService(IHttpClientFactory clientFactory): base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> getPosition<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.GET,
                Url=SD.identityApiBase+ "/api/position/getPosition/"+id
            });
        }

        public async Task<T> insertPosition<T>(setPosition setPositio)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data=setPositio,
                Url=SD.identityApiBase+ "/api/position/insertPosition"
            });
        }
    }
}
