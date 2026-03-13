using webapi.Model.Workflow;

namespace webapi.Services.IServices.IWorkflowService
{
    public interface IreferralWorkflowService:IBaseService
    {
        Task<T> createreferral<T>(setReferralDTO setReferralDTO);
        Task<T> getAllByPositon<T>(int id);
        Task<T> getAllByReciver<T>(int id);
        Task<T> getreferral<T>(int id);
        Task<T> getAll<T>();
    }
}
