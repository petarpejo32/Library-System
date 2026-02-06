using Xunit;
using Moq;
using LibrarySystem.Service.Implementation;
using LibrarySystem.Repository.Interface;
using LibrarySystem.Domain.Models;
using FluentAssertions;
using System.Linq.Expressions;

namespace LibrarySystem.Tests.Unit.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IRepository<Book>> _mockRepo;
        private readonly BookService _service;

        public BookServiceTests()
        {
            _mockRepo = new Mock<IRepository<Book>>();
            _service = new BookService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllBooks_ShouldReturnBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Book 1", ISBN = "1111111111111" },
                new Book { Id = Guid.NewGuid(), Title = "Book 2", ISBN = "2222222222222" }
            };

            _mockRepo.Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns(books);

            var result = _service.GetAllBooks();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetBookById_WhenExists_ShouldReturnBook()
        {
            var id = Guid.NewGuid();
            var book = new Book { Id = id, Title = "Test Book", ISBN = "1234567890123" };

            _mockRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns(book);

            var result = _service.GetBookById(id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
        }

        [Fact]
        public void GetBookById_WhenNotExists_ShouldReturnNull()
        {
            _mockRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns((Book?)null);

            var result = _service.GetBookById(Guid.NewGuid());

            result.Should().BeNull();
        }

        [Fact]
        public void CreateBook_ShouldGenerateIdAndCallInsert()
        {
            var book = new Book
            {
                Title = "New Book",
                ISBN = "1234567890123",
                AuthorId = Guid.NewGuid(),
                TotalCopies = 5,
                AvailableCopies = 5
            };

            _mockRepo.Setup(r => r.Insert(It.IsAny<Book>())).Returns(book);

            _service.CreateBook(book);

            book.Id.Should().NotBe(Guid.Empty);
            _mockRepo.Verify(r => r.Insert(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public void UpdateBook_ShouldCallUpdate()
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Updated Book",
                ISBN = "1234567890123"
            };

            _mockRepo.Setup(r => r.Update(It.IsAny<Book>())).Returns(book);

            _service.UpdateBook(book);

            _mockRepo.Verify(r => r.Update(It.Is<Book>(b => b.Id == book.Id)), Times.Once);
        }

        [Fact]
        public void DeleteBook_ShouldCallDelete()
        {
            var id = Guid.NewGuid();
            var book = new Book { Id = id, Title = "To Delete" };

            _mockRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns(book);

            _mockRepo.Setup(r => r.Delete(It.IsAny<Book>())).Returns(book);

            _service.DeleteBook(id);

            _mockRepo.Verify(r => r.Delete(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public void GetAvailableBooks_ShouldReturnOnlyBooksWithAvailableCopies()
        {
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Available 1", AvailableCopies = 5 },
                new Book { Id = Guid.NewGuid(), Title = "Available 2", AvailableCopies = 2 }
            };

            _mockRepo.Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns(books);

            var result = _service.GetAvailableBooks();

            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetBooksByAuthor_ShouldReturnAuthorBooks()
        {
            var authorId = Guid.NewGuid();
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Book 1", AuthorId = authorId },
                new Book { Id = Guid.NewGuid(), Title = "Book 2", AuthorId = authorId }
            };

            _mockRepo.Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns(books);

            var result = _service.GetBooksByAuthor(authorId);

            result.Should().HaveCount(2);
        }
    }
}