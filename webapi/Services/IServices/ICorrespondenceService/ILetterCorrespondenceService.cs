using webapi.Model.CorrespondenceModel;

namespace webapi.Services.IServices.ICorrespondenceService
{
    public interface ILetterCorrespondenceService:IBaseService
    {
        Task<T> addLetter<T>(setLetterDTO letterDTO);
        Task<T> getLetter<T>(int id);
        Task<T> getLetters<T>();
        Task<T> updateLetter<T>(LetterDTO letterDTO);
    }
}
