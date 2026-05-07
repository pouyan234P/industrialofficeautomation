using webapi.Model;
using webapi.Model.CorrespondenceModel.Enum;
using webapi.Model.Workflow;

namespace webapi.Services.IServices.IWorkflowService
{
    public interface IreferralWorkflowService:IBaseService
    {
        Task<T> createreferral<T>(setReferralDTO setReferralDTO);
        Task<T> getAllByPositon<T>(int id);
        Task<T> getAllByReciver<T>(int id, UserParams userParams);
        Task<T> getreferral<T>(int id);
        Task<T> getAll<T>();
        Task<T> getbytyperecvierid<T>(int reciveid, TypeDTO type, UserParams userParams);
        Task<T> getAllbySenderposition<T>(int senderid, UserParams userParams);
        Task<T> getReferralbyTypeSenderid<T>(int senderid, TypeDTO type, UserParams userParams);
    }
}
