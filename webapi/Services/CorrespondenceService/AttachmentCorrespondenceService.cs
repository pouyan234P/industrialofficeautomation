using webapi.Model;
using webapi.Model.CorrespondenceModel;
using webapi.Services.IServices.ICorrespondenceService;

namespace webapi.Services.CorrespondenceService
{
    public class AttachmentCorrespondenceService : BaseService, IAttachmentCorrespondenceService
    {
        private readonly IHttpClientFactory _clientFactory;

        public AttachmentCorrespondenceService(IHttpClientFactory clientFactory): base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> addAttachment<T>(setAttachmentDTO attachmentDTO)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data=attachmentDTO,
                Url=SD.gatewayApiBase + "/api/attachment/addAttachment"
            });
        }

        public async Task<T> getAttachment<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.GET,
                Url=SD.gatewayApiBase + "/api/attachment/getAttachment"+ id
            });
        }

        public async Task<T> getAttachments<T>()
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType= SD.ApiType.GET,
                Url=SD.gatewayApiBase + "/api/attachment/getAttachments"
            });
        }

        public async Task<T> updateAttachment<T>(AttachmentDTO attachmentDTO)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data=attachmentDTO,
                Url=SD.gatewayApiBase + "/api/attachment/updateAttachment"
            });
        }
    }
}
