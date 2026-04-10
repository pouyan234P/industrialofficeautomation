using webapi.Model;
using webapi.Model.CorrespondenceModel.Enum;
using webapi.Model.Workflow;
using webapi.Services.IServices.IWorkflowService;

namespace webapi.Services.WorkflowService
{
    public class referralWorkflowService : BaseService, IreferralWorkflowService
    {
        private readonly IHttpClientFactory _clientFactory;

        public referralWorkflowService(IHttpClientFactory clientFactory): base(clientFactory)
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

        public async Task<T> getAllByReciver<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url=SD.gatewayApiBase+ "/api/referral/getAllByReciver/"+ id
            });
        }

        public async Task<T> getbytyperecvierid<T>(int reciveid, TypeDTO type)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data=type,
                Url=SD.gatewayApiBase+ "/api/referral/getbytyperecvierid/"+reciveid
            });
        }

        public async Task<T> getreferral<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType= SD.ApiType.GET,
                Url=SD.gatewayApiBase+ "/api/referral/getreferral"+id
            });
        }
    }
}
