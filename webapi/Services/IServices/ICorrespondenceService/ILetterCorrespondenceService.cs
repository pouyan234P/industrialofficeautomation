using webapi.Model.CorrespondenceModel;
using webapi.Model.CorrespondenceModel.Enum;

namespace webapi.Services.IServices.ICorrespondenceService
{
    public interface ILetterCorrespondenceService:IBaseService
    {
        Task<T> addLetter<T>(setLetterDTO letterDTO);
        Task<T> getLetter<T>(int id);
        Task<T> getLetters<T>();
        Task<T> GetLetterbyType<T>(TypeDTO type);
        Task<T> updateLetter<T>(LetterDTO letterDTO);
        Task<T> Ping<T>();
    }
}
