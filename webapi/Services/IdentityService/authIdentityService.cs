using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Services.IdentityService
{
    public class authIdentityService : BaseService, IauthIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;

        public authIdentityService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor) : base(clientFactory, httpContextAccessor) 
        {
            _clientFactory = clientFactory;
        }
        public Task<T> CreateRole<T>(string myroleName)
            => createRole<T>(new createRoleDTO { name = myroleName });

        public async Task<T> createRole<T>(createRoleDTO dto)
        {
            // Role name travels in the JSON body, not the URL, so it can't alter the path.
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = SD.identityApiBase + "/api/auth/roles"
            });
        }

        public async Task<T> getRoles<T>()
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.identityApiBase + "/api/auth/roles"
            });
        }

        public async Task<T> setUserRoles<T>(int userId, setUserRolesDTO dto)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = SD.identityApiBase + "/api/auth/users/" + userId + "/roles"
            });
        }

        public async Task<T> getAll<T>()
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url=SD.identityApiBase+ "/api/auth/getAll"
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

        public async Task<T> updateUser<T>(int id, updateUserDTO dto)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = SD.identityApiBase + "/api/auth/updateUser/" + id
            });
        }
    }
}
