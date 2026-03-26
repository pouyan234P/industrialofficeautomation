using webapi.Model.Authentication;

namespace webapi.Services.IServices.Identity
{
    public interface IauthIdentityService:IBaseService
    {
        Task<T> login<T>(loginDTO mylogin);
        Task<T> register<T>(registerDTO myregisterDTO);
        Task<T> CreateRole<T>(string myroleName);
        Task<T> getAll<T>();
    }
}
