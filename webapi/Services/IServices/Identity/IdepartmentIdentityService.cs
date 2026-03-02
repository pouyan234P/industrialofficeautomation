using webapi.Model.Authentication;

namespace webapi.Services.IServices.Identity
{
    public interface IdepartmentIdentityService:IBaseService
    {
        Task<T> addDepartment<T>(DepartmentDTO mydepartmentDTO);
        Task<T> getDepartmentbyid<T>(int id);
        Task<T> getDepartment<T>();
        Task<T> getDepartmentbyname<T>(string name);

    }
}
