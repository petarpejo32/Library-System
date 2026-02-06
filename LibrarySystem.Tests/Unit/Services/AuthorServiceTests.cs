using Xunit;
using Moq;
using LibrarySystem.Service.Implementation;
using LibrarySystem.Repository.Interface;
using LibrarySystem.Domain.Models;
using FluentAssertions;
using System.Linq.Expressions;

namespace LibrarySystem.Tests.Unit.Services
{
    public class AuthorServiceTests
    {
        private readonly Mock<IRepository<Author>> _mockRepo;
        private readonly AuthorService _service;

        public AuthorServiceTests()
        {
            _mockRepo = new Mock<IRepository<Author>>();
            _service = new AuthorService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllAuthors_ShouldReturnAllAuthors()
        {
            var authors = new List<Author>
            {
                new Author { Id = Guid.NewGuid(), Name = "Author 1" },
                new Author { Id = Guid.NewGuid(), Name = "Author 2" }
            };

            _mockRepo.Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Author, Author>>>(),
                It.IsAny<Expression<Func<Author, bool>>>(),
                It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(),
                It.IsAny<Func<IQueryable<Author>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Author, object>>>()
            )).Returns(authors);

            var result = _service.GetAllAuthors();

            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetAuthorById_WhenExists_ShouldReturnAuthor()
        {
            var id = Guid.NewGuid();
            var author = new Author { Id = id, Name = "Test Author" };

            _mockRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Author, Author>>>(),
                It.IsAny<Expression<Func<Author, bool>>>(),
                It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(),
                It.IsAny<Func<IQueryable<Author>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Author, object>>>()
            )).Returns(author);

            var result = _service.GetAuthorById(id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Author");
        }

        [Fact]
        public void GetAuthorById_WhenNotExists_ShouldReturnNull()
        {
            _mockRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Author, Author>>>(),
                It.IsAny<Expression<Func<Author, bool>>>(),
                It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(),
                It.IsAny<Func<IQueryable<Author>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Author, object>>>()
            )).Returns((Author?)null);

            var result = _service.GetAuthorById(Guid.NewGuid());

            result.Should().BeNull();
        }

        [Fact]
        public void CreateAuthor_ShouldGenerateIdAndCallInsert()
        {
            var author = new Author { Name = "New Author" };
            _mockRepo.Setup(r => r.Insert(It.IsAny<Author>())).Returns(author);

            _service.CreateAuthor(author);

            author.Id.Should().NotBe(Guid.Empty);
            _mockRepo.Verify(r => r.Insert(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public void UpdateAuthor_ShouldCallUpdate()
        {
            var author = new Author { Id = Guid.NewGuid(), Name = "Updated Author" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Author>())).Returns(author);

            _service.UpdateAuthor(author);

            _mockRepo.Verify(r => r.Update(It.Is<Author>(a => a.Id == author.Id)), Times.Once);
        }

        [Fact]
        public void DeleteAuthor_ShouldCallDelete()
        {
            var id = Guid.NewGuid();
            var author = new Author { Id = id, Name = "To Delete" };

            _mockRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Author, Author>>>(),
                It.IsAny<Expression<Func<Author, bool>>>(),
                It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(),
                It.IsAny<Func<IQueryable<Author>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Author, object>>>()
            )).Returns(author);

            _mockRepo.Setup(r => r.Delete(It.IsAny<Author>())).Returns(author);

            _service.DeleteAuthor(id);

            _mockRepo.Verify(r => r.Delete(It.IsAny<Author>()), Times.Once);
        }
    }
}