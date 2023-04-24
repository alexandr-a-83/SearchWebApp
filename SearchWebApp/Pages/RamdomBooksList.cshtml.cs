using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SearchWebApp.Models;
using SearchWebApp.Services;
using System.Configuration;
using static System.Reflection.Metadata.BlobBuilder;

namespace SearchWebApp.Pages
{
    public class RamdomBooksListModel : PageModel
    {
        DataProvider _dataProvider;

        [BindProperty]
        public int NumberOfRecords
        {
            get; set;
        }

        public List<BookModel> Books
        {
            get; set;
        }

        public PaginatedList<BookModel> PaginedBooks
        {
            get; set;
        }

        public RamdomBooksListModel(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;                       
            Books = dataProvider.books.ToList<BookModel>();
            NumberOfRecords = Books.Count();
        }

        public async Task OnGetAsync(int? pageIndex)
        {  

            IQueryable<BookModel> booksIQ = from s in _dataProvider.books.AsQueryable<BookModel>() select s;

            var pageSize = 50;
            PaginedBooks = await PaginatedList<BookModel>.CreateAsync(
                booksIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

        }

    }
}
