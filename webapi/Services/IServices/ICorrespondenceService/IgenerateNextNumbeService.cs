using webapi.Model.CorrespondenceModel;
using webapi.Model.CorrespondenceModel.Enum;

namespace webapi.Services.IServices.ICorrespondenceService
{
    public interface IgenerateNextNumbeService:IBaseService
    {
        Task<T> mynextnumber<T>(getNextNumberDTO dto);
    }
}
