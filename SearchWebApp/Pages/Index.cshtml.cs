using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SearchWebApp.Models;
using SearchWebApp.Services;
using System.Linq;

namespace SearchWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public int ResulnCounter
        {
            get; set;
        }

        DataProvider _dataProvider;

        [BindProperty]
        public List<BookModel> Books
        {
            get; set;
        }        

        [BindProperty]
        public string SearchValue
        {
            get; set;
        }        

        public int CurrentPage { get; set; } = 1;
        
        public PaginatedList<BookModel> PaginedBooks
        {
            get; set;
        }

        public IndexModel(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            Books = _dataProvider.books.ToList<BookModel>();
        }

        public async Task OnGetAsync(int? pageIndex)
        {

            Books = _dataProvider.searchResult.ToList<BookModel>();
            
            ResulnCounter = Books.Count();

            IQueryable<BookModel> booksIQ = from s in Books.AsQueryable<BookModel>() select s;

            var pageSize = 50;
            PaginedBooks = await PaginatedList<BookModel>.CreateAsync(
                booksIQ.AsNoTracking(), pageIndex ?? 1, pageSize);          

        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid.Equals(false))
            {
                SearchValue = "";                
            }



            _dataProvider.GetSearchResult((SearchValue.Trim() + " ").Replace(" ", "* ").Trim());            

            Books = _dataProvider.searchResult.ToList<BookModel>();             
            ResulnCounter = Books.Count();

            IQueryable<BookModel> booksIQ = from s in Books.AsQueryable<BookModel>() select s;

            var pageSize = 50;            

            PaginedBooks = await PaginatedList<BookModel>.CreateAsync(
                booksIQ.AsNoTracking(), 1, pageSize);            

        }
    }
}