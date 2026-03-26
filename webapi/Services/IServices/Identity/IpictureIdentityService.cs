using authentication.application.DTO;
using webapi.Model.Authentication;

namespace webapi.Services.IServices.Identity
{
    public interface IpictureIdentityService
    {
        Task<T> addPicture<T>(IFormFile dTO);
        Task<getsignitureimageDTO> getPicture(int id);
    }
}
