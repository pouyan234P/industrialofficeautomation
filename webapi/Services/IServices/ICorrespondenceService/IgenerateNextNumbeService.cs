using webapi.Model.CorrespondenceModel;
using webapi.Model.CorrespondenceModel.Enum;

namespace webapi.Services.IServices.ICorrespondenceService
{
    public interface IgenerateNextNumbeService:IBaseService
    {
        Task<T> mynextnumber<T>(getNextNumberDTO dto);
        Task<T> getlastNumberbyType<T>(TypeDTO type,int depid,int year);
    }
}
