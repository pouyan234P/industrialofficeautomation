using webapi.Model;

namespace webapi.Services.IServices
{
    public interface IBaseService:IDisposable
    {
        ResponseDTO responseDTO { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
