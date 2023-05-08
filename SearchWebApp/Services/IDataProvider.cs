using SearchWebApp.Models;

namespace SearchWebApp.Services
{
    public interface IDataProvider
    {
        IEnumerable<BookModel> GetData(int pageNumber, int pageSize);
        void GetSearchResult(string searchValue);
    }
}