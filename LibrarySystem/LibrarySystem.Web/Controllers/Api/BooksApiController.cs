using LibrarySystem.Domain.DTOs;
using LibrarySystem.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Web.Controllers.Api
{
    [Route("api/books")]
    [ApiController]
    public class BooksApiController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksApiController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/books
        [HttpGet]
        public ActionResult<IEnumerable<BookDTO>> GetAllBooks()
        {
            var books = _bookService.GetAllBooks();
            var bookDTOs = books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublishedYear = b.PublishedYear,
                Genre = b.Genre,
                TotalCopies = b.TotalCopies,
                AvailableCopies = b.AvailableCopies,
                AuthorName = b.Author?.Name ?? "Unknown",
                AuthorId = b.AuthorId
            }).ToList();

            return Ok(bookDTOs);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBook(Guid id)
        {
            var book = _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            var bookDTO = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear,
                Genre = book.Genre,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                AuthorName = book.Author?.Name ?? "Unknown",
                AuthorId = book.AuthorId
            };

            return Ok(bookDTO);
        }

        // GET: api/books/available
        [HttpGet("available")]
        public ActionResult<IEnumerable<BookDTO>> GetAvailableBooks()
        {
            var books = _bookService.GetAvailableBooks();
            var bookDTOs = books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublishedYear = b.PublishedYear,
                Genre = b.Genre,
                TotalCopies = b.TotalCopies,
                AvailableCopies = b.AvailableCopies,
                AuthorName = b.Author?.Name ?? "Unknown",
                AuthorId = b.AuthorId
            }).ToList();

            return Ok(bookDTOs);
        }

        // GET: api/books/author/{authorId}
        [HttpGet("author/{authorId}")]
        public ActionResult<IEnumerable<BookDTO>> GetBooksByAuthor(Guid authorId)
        {
            var books = _bookService.GetBooksByAuthor(authorId);
            var bookDTOs = books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublishedYear = b.PublishedYear,
                Genre = b.Genre,
                TotalCopies = b.TotalCopies,
                AvailableCopies = b.AvailableCopies,
                AuthorName = b.Author?.Name ?? "Unknown",
                AuthorId = b.AuthorId
            }).ToList();

            return Ok(bookDTOs);
        }

        // POST: api/books
        [HttpPost]
        public ActionResult<BookDTO> CreateBook([FromBody] BookDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = new Domain.Models.Book
            {
                Title = bookDTO.Title,
                ISBN = bookDTO.ISBN,
                PublishedYear = bookDTO.PublishedYear,
                Genre = bookDTO.Genre,
                TotalCopies = bookDTO.TotalCopies,
                AvailableCopies = bookDTO.AvailableCopies,
                AuthorId = bookDTO.AuthorId
            };

            _bookService.CreateBook(book);

            bookDTO.Id = book.Id;
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDTO);
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBook(Guid id, [FromBody] BookDTO bookDTO)
        {
            if (id != bookDTO.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var existingBook = _bookService.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            existingBook.Title = bookDTO.Title;
            existingBook.ISBN = bookDTO.ISBN;
            existingBook.PublishedYear = bookDTO.PublishedYear;
            existingBook.Genre = bookDTO.Genre;
            existingBook.TotalCopies = bookDTO.TotalCopies;
            existingBook.AvailableCopies = bookDTO.AvailableCopies;
            existingBook.AuthorId = bookDTO.AuthorId;

            _bookService.UpdateBook(existingBook);

            return NoContent();
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(Guid id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            _bookService.DeleteBook(id);
            return NoContent();
        }
    }
}