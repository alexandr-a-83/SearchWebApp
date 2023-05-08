using Bogus;
using SearchWebApp.Models;
using System.Configuration;

namespace SearchWebApp.Services
{
    public class DataGenerator
    {

        private readonly IConfiguration Configuration;
        private int useSeed;

        Faker<BookModel> _bookFaker;  
        
        IEnumerable<BookModel> _books;        

        public DataGenerator(IConfiguration configuration)
        {
            Configuration = configuration;
            var useSeed = Configuration.GetValue("FakerDBNumber", 2000);

            _bookFaker = new Faker<BookModel>()
                
                // Method for creating same DB
                .UseSeed(useSeed)

                .RuleFor(b => b.Id, f => f.IndexFaker)
                .RuleFor(b => b.Name, f => f.Name.JobTitle())
                .RuleFor(b => b.Author, f => f.Name.FullName())
                .RuleFor(b => b.Description, f => f.Lorem.Sentences());
        }       

        public IEnumerable<BookModel> GenerateBooks(int numberOfRecords)
        {
            if (_books is null)
            {
                _books = new List<BookModel>();
                _books = _bookFaker.Generate(numberOfRecords).AsEnumerable();
            }

            return _books;
        }

    }
}
