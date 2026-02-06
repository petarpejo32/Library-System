using LibrarySystem.Domain.Models;

namespace LibrarySystem.Service.Interface
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book? GetBookById(Guid id);
        void CreateBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(Guid id);

        List<Book> GetAvailableBooks();
        List<Book> GetBooksByAuthor(Guid authorId);
    }
}