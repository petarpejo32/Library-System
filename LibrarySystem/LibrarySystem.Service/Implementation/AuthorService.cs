using LibrarySystem.Domain.Models;
using LibrarySystem.Repository.Interface;
using LibrarySystem.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Service.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public List<Author> GetAllAuthors()
        {
            return _authorRepository.GetAll(
                author => author,
                include: query => query.Include(a => a.Books)
            ).ToList();
        }

        public Author? GetAuthorById(Guid id)
        {
            return _authorRepository.Get(
                author => author,
                predicate: a => a.Id == id,
                include: query => query.Include(a => a.Books)
            );
        }

        public void CreateAuthor(Author author)
        {
            author.Id = Guid.NewGuid();
            _authorRepository.Insert(author);
        }

        public void UpdateAuthor(Author author)
        {
            _authorRepository.Update(author);
        }

        public void DeleteAuthor(Guid id)
        {
            var author = GetAuthorById(id);
            if (author != null)
            {
                _authorRepository.Delete(author);
            }
        }
    }
}