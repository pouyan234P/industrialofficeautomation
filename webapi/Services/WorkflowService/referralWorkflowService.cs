using webapi.Model;
using webapi.Model.CorrespondenceModel.Enum;
using webapi.Model.Workflow;
using webapi.Services.IServices.IWorkflowService;

namespace webapi.Services.WorkflowService
{
    public class referralWorkflowService : BaseService, IreferralWorkflowService
    {
        private readonly IHttpClientFactory _clientFactory;

        public referralWorkflowService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor) : base(clientFactory, httpContextAccessor)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> createreferral<T>(setReferralDTO setReferralDTO)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data = setReferralDTO,
                Url=SD.gatewayApiBase+ "/api/referral/createreferral"
            });
        }

        public async Task<T> getAll<T>()
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.GET,
                Url= SD.gatewayApiBase+ "/api/referral/getAll"
            });
        }

        public async Task<T> getAllByPositon<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url=SD.gatewayApiBase+ "/api/referral/getAllByPositon/"+id
            });
        }

        public async Task<T> getAllByReciver<T>(int id, UserParams userParams)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url=SD.gatewayApiBase+ "/api/referral/getAllByReciver/"+ id + "?" + "PageNumber=" + userParams.PageNumber
            });
        }

        public async Task<T> getAllbySenderposition<T>(int senderid, UserParams userParams)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.GET,
                Url=SD.gatewayApiBase+ "/api/referral/getAllbySenderposition/"+ senderid + "?" + "PageNumber=" + userParams.PageNumber
            });
        }

        public async Task<T> getbytyperecvierid<T>(int reciveid, TypeDTO type, UserParams userParams)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data=type,
                Url=SD.gatewayApiBase + "/api/referral/getbytyperecvierid/" + reciveid + "?" + "PageNumber=" + userParams.PageNumber
            });
        }

        public async Task<T> getreferral<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType= SD.ApiType.GET,
                Url=SD.gatewayApiBase+ "/api/referral/getreferral/"+id
            });
        }

        public async Task<T> getReferralbyTypeSenderid<T>(int senderid, TypeDTO type, UserParams userParams)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data=type,
                Url=SD.gatewayApiBase + "/api/referral/getReferralbyTypeSenderid/" + senderid + "?" + "PageNumber=" + userParams.PageNumber
            });
        }
    }
}
