using webapi.Model.CorrespondenceModel;

namespace webapi.Services.IServices.ICorrespondenceService
{
    public interface IAttachmentCorrespondenceService:IBaseService
    {
        Task<T> addAttachment<T>(setAttachmentDTO attachmentDTO);
        Task<T> getAttachment<T>(int id);
        Task<T> getAttachments<T>();
        Task<T> updateAttachment<T>(AttachmentDTO attachmentDTO);
    }
}
