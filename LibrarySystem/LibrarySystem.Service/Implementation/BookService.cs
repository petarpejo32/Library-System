using LibrarySystem.Domain.Models;
using LibrarySystem.Repository.Interface;
using LibrarySystem.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Service.Implementation
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAll(
                book => book,
                include: query => query.Include(b => b.Author)
            ).ToList();
        }

        public Book? GetBookById(Guid id)
        {
            return _bookRepository.Get(
                book => book,
                predicate: b => b.Id == id,
                include: query => query.Include(b => b.Author)
            );
        }

        public void CreateBook(Book book)
        {
            book.Id = Guid.NewGuid();
            _bookRepository.Insert(book);
        }

        public void UpdateBook(Book book)
        {
            _bookRepository.Update(book);
        }

        public void DeleteBook(Guid id)
        {
            var book = GetBookById(id);
            if (book != null)
            {
                _bookRepository.Delete(book);
            }
        }

        public List<Book> GetAvailableBooks()
        {
            return _bookRepository.GetAll(
                book => book,
                predicate: b => b.AvailableCopies > 0,
                include: query => query.Include(b => b.Author)
            ).ToList();
        }

        public List<Book> GetBooksByAuthor(Guid authorId)
        {
            return _bookRepository.GetAll(
                book => book,
                predicate: b => b.AuthorId == authorId,
                include: query => query.Include(b => b.Author)
            ).ToList();
        }
    }
}