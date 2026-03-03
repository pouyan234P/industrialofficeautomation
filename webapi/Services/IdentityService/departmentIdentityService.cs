using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Services.IdentityService
{
    public class departmentIdentityService : BaseService, IdepartmentIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;

        public departmentIdentityService(IHttpClientFactory clientFactory): base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> addDepartment<T>(DepartmentDTO mydepartmentDTO)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data = mydepartmentDTO,
                Url=SD.identityApiBase+ "/api/department/addDepartment"
            });
        }

        public async Task<T> getDepartment<T>()
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url=SD.identityApiBase+ "/api/department/getDepartment"
            });
        }

        public async Task<T> getDepartmentbyid<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.GET,
                Url= SD.identityApiBase+ "/api/department/getDepartmentbyid/"+id
            });
        }

        public async Task<T> getDepartmentbyname<T>(string name)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.GET,
                Url=SD.identityApiBase+ "/api/department/getDepartmentbyname/"+name
            });
        }
    }
}
