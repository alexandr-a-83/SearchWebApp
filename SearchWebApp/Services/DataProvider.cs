using Microsoft.AspNetCore.Mvc;
using SearchWebApp.Models;
using System.Xml.Linq;

namespace SearchWebApp.Services
{
    public class DataProvider : IDataProvider
    {
        private readonly IConfiguration Configuration;

        public int numberOfRecords;

        public IEnumerable<BookModel> books;

        public IEnumerable<BookModel> searchResult;

        SearchEngine searchEngine;

        public DataProvider(IConfiguration configuration)
        {
            Configuration = configuration;
            numberOfRecords = Configuration.GetValue("NumberOfRecords", 5000);

            GetBooksList();
            searchResult = new List<BookModel>();

            searchEngine = new SearchEngine(books);

        }

        private void GetBooksList()
        {
            if (books is null || books.ToList().Count == 0)
            {
                books = new List<BookModel>();

                DataGenerator dataGenerator = new DataGenerator(Configuration);
                books = dataGenerator.GenerateBooks(numberOfRecords);
            }
        }

        public void GetSearchResult(string searchValue)
        {
            searchEngine.numHits = numberOfRecords;
            searchResult = searchEngine.Search(searchValue);
        }

        public IEnumerable<BookModel> GetData(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            return searchResult.Skip(skip).Take(pageSize).ToList();
        }


    }
}
