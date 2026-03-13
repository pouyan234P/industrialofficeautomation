using webapi.Model.Searchengine;

namespace webapi.Services.IServices.ISearchEngineService
{
    public interface IletterelsiSearchEngineService
    {
        Task<T> search<T>(SearchRequestDto searchRequest);
    }
}
