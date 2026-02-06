using LibrarySystem.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Web.Controllers
{
    public class ExternalBooksController : Controller
    {
        private readonly IExternalApiService _externalApiService;
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;

        public ExternalBooksController(
            IExternalApiService externalApiService,
            IAuthorService authorService,
            IBookService bookService)
        {
            _externalApiService = externalApiService;
            _authorService = authorService;
            _bookService = bookService;
        }

        // GET: ExternalBooks/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: ExternalBooks/Search
        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                TempData["ErrorMessage"] = "Please enter a search term.";
                return View();
            }

            var books = await _externalApiService.SearchBooksAsync(query);

            if (books == null || !books.Any())
            {
                TempData["InfoMessage"] = $"No books found for '{query}'.";
                return View();
            }

            return View("SearchResults", books);
        }

        // GET: ExternalBooks/ImportBook
        public async Task<IActionResult> ImportBook(string title, string author, string isbn, int? year, string? genre)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
            {
                TempData["ErrorMessage"] = "Title and Author are required to import a book.";
                return RedirectToAction(nameof(Search));
            }

            // Check if author exists, if not create one
            var authors = _authorService.GetAllAuthors();
            var existingAuthor = authors.FirstOrDefault(a =>
                a.Name.Equals(author, StringComparison.OrdinalIgnoreCase));

            Guid authorId;
            if (existingAuthor == null)
            {
                var newAuthor = new Domain.Models.Author
                {
                    Name = author,
                    Biography = $"Author imported from Open Library API",
                    Nationality = null,
                    BirthYear = null
                };
                _authorService.CreateAuthor(newAuthor);
                authorId = newAuthor.Id;
            }
            else
            {
                authorId = existingAuthor.Id;
            }

            // Create the book
            var book = new Domain.Models.Book
            {
                Title = title,
                ISBN = isbn ?? "N/A",
                PublishedYear = year ?? DateTime.Now.Year,
                Genre = genre,
                TotalCopies = 1,
                AvailableCopies = 1,
                AuthorId = authorId
            };

            _bookService.CreateBook(book);

            TempData["SuccessMessage"] = $"Book '{title}' imported successfully!";
            return RedirectToAction("Index", "Books");
        }
    }
}