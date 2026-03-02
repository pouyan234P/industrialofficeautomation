using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Services.IdentityService
{
    public class authIdentityService : BaseService, IauthIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;

        public authIdentityService(IHttpClientFactory clientFactory): base(clientFactory) 
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> CreateRole<T>(string myroleName)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Url=SD.identityApiBase+ "/api/auth/CreateRole"+myroleName
            });
        }

        public async Task<T> login<T>(loginDTO mylogin)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data=mylogin,
                Url=SD.identityApiBase+ "/api/auth/login"
            });
        }

        public async Task<T> register<T>(registerDTO myregisterDTO)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data=myregisterDTO,
                Url=SD.identityApiBase+ "/api/auth/register"
            });
        }
    }
}
