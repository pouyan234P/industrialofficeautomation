
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Services.IdentityService
{
    public class pictureIdentityService : BaseService, IpictureIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;

        public pictureIdentityService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> addPicture<T>(IFormFile dto)
        {
            

            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = new {file=dto},
                Url = SD.identityApiBase + "/api/picture/addPicture",
                ContentType = "multipart/form-data"
            });
        }

        public async Task<getsignitureimageDTO> getPicture(int id)
        {
            var (data, contentType) = await this.SendFileAsync(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.identityApiBase + "/api/picture/getPicture/" + id
            });

            return new getsignitureimageDTO
            {
                ImageData = data,
                ContentType = contentType
            };
        }
    }
}