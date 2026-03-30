using webapi.Model.Authentication;

namespace webapi.Services.IServices.Identity
{
    public interface IpositionIdentityService:IBaseService
    {
        Task<T> insertPosition<T>(setPosition setPositio);
        Task<T> getPosition<T>(int id);
        Task<T> getAll<T>();
    }
}
