using LibrarySystem.Domain.Models;

namespace LibrarySystem.Service.Interface
{
    public interface IAuthorService
    {
        List<Author> GetAllAuthors();
        Author? GetAuthorById(Guid id);
        void CreateAuthor(Author author);
        void UpdateAuthor(Author author);
        void DeleteAuthor(Guid id);
    }
}