using Xunit;
using Moq;
using LibrarySystem.Service.Implementation;
using LibrarySystem.Repository.Interface;
using LibrarySystem.Domain.Models;
using FluentAssertions;
using System.Linq.Expressions;

namespace LibrarySystem.Tests.Unit.Services
{
    public class BorrowingServiceTests
    {
        private readonly Mock<IRepository<Borrowing>> _mockBorrowingRepo;
        private readonly Mock<IRepository<Book>> _mockBookRepo;
        private readonly Mock<IRepository<Member>> _mockMemberRepo;
        private readonly BorrowingService _service;

        public BorrowingServiceTests()
        {
            _mockBorrowingRepo = new Mock<IRepository<Borrowing>>();
            _mockBookRepo = new Mock<IRepository<Book>>();
            _mockMemberRepo = new Mock<IRepository<Member>>();
            _service = new BorrowingService(
                _mockBorrowingRepo.Object,
                _mockBookRepo.Object,
                _mockMemberRepo.Object
            );
        }

        [Fact]
        public void GetAllBorrowings_ShouldReturnAllBorrowings()
        {
            var borrowings = new List<Borrowing>
            {
                new Borrowing { Id = Guid.NewGuid() },
                new Borrowing { Id = Guid.NewGuid() }
            };

            _mockBorrowingRepo.Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Borrowing, Borrowing>>>(),
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, IOrderedQueryable<Borrowing>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Borrowing, object>>>()
            )).Returns(borrowings);

            var result = _service.GetAllBorrowings();

            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetBorrowingById_WhenExists_ShouldReturnBorrowing()
        {
            var id = Guid.NewGuid();
            var borrowing = new Borrowing { Id = id, IsReturned = false };

            _mockBorrowingRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Borrowing, Borrowing>>>(),
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, IOrderedQueryable<Borrowing>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Borrowing, object>>>()
            )).Returns(borrowing);

            var result = _service.GetBorrowingById(id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
        }

        [Fact]
        public void GetActiveBorrowings_ShouldReturnOnlyUnreturned()
        {
            var borrowings = new List<Borrowing>
            {
                new Borrowing { Id = Guid.NewGuid(), IsReturned = false },
                new Borrowing { Id = Guid.NewGuid(), IsReturned = false }
            };

            _mockBorrowingRepo.Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Borrowing, Borrowing>>>(),
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, IOrderedQueryable<Borrowing>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Borrowing, object>>>()
            )).Returns(borrowings);

            var result = _service.GetActiveBorrowings();

            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetOverdueBorrowings_ShouldReturnOnlyOverdue()
        {
            var borrowings = new List<Borrowing>
            {
                new Borrowing { Id = Guid.NewGuid(), DueDate = DateTime.Now.AddDays(-5), IsReturned = false },
                new Borrowing { Id = Guid.NewGuid(), DueDate = DateTime.Now.AddDays(-2), IsReturned = false }
            };

            _mockBorrowingRepo.Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Borrowing, Borrowing>>>(),
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, IOrderedQueryable<Borrowing>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Borrowing, object>>>()
            )).Returns(borrowings);

            var result = _service.GetOverdueBorrowings();

            result.Should().HaveCount(2);
        }

        [Fact]
        public void CreateBorrowing_ShouldGenerateIdAndCallInsert()
        {
            var borrowing = new Borrowing
            {
                BookId = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                BorrowDate = DateTime.Now
            };

            _mockBorrowingRepo.Setup(r => r.Insert(It.IsAny<Borrowing>())).Returns(borrowing);

            _service.CreateBorrowing(borrowing);

            borrowing.Id.Should().NotBe(Guid.Empty);
            _mockBorrowingRepo.Verify(r => r.Insert(It.IsAny<Borrowing>()), Times.Once);
        }

        [Fact]
        public void UpdateBorrowing_ShouldCallUpdate()
        {
            var borrowing = new Borrowing { Id = Guid.NewGuid() };
            _mockBorrowingRepo.Setup(r => r.Update(It.IsAny<Borrowing>())).Returns(borrowing);

            _service.UpdateBorrowing(borrowing);

            _mockBorrowingRepo.Verify(r => r.Update(It.Is<Borrowing>(b => b.Id == borrowing.Id)), Times.Once);
        }

        [Fact]
        public void DeleteBorrowing_ShouldCallDelete()
        {
            var id = Guid.NewGuid();
            var borrowing = new Borrowing { Id = id, IsReturned = true };

            _mockBorrowingRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Borrowing, Borrowing>>>(),
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, IOrderedQueryable<Borrowing>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Borrowing, object>>>()
            )).Returns(borrowing);

            _mockBorrowingRepo.Setup(r => r.Delete(It.IsAny<Borrowing>())).Returns(borrowing);

            _service.DeleteBorrowing(id);

            _mockBorrowingRepo.Verify(r => r.Delete(It.IsAny<Borrowing>()), Times.Once);
        }

        [Fact]
        public void BorrowBook_WithAvailableBook_ShouldCreateBorrowing()
        {
            var bookId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var book = new Book { Id = bookId, Title = "Test Book", AvailableCopies = 5 };
            var member = new Member { Id = memberId, Name = "Test Member" };

            _mockBookRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns(book);

            _mockMemberRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Member, Member>>>(),
                It.IsAny<Expression<Func<Member, bool>>>(),
                It.IsAny<Func<IQueryable<Member>, IOrderedQueryable<Member>>>(),
                It.IsAny<Func<IQueryable<Member>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Member, object>>>()
            )).Returns(member);

            _mockBorrowingRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Borrowing, Borrowing>>>(),
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, IOrderedQueryable<Borrowing>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Borrowing, object>>>()
            )).Returns((Borrowing?)null);

            _mockBookRepo.Setup(r => r.Update(It.IsAny<Book>())).Returns(book);
            _mockBorrowingRepo.Setup(r => r.Insert(It.IsAny<Borrowing>())).Returns(new Borrowing());

            var result = _service.BorrowBook(bookId, memberId);

            result.Should().NotBeNull();
            _mockBorrowingRepo.Verify(r => r.Insert(It.IsAny<Borrowing>()), Times.Once);
            _mockBookRepo.Verify(r => r.Update(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public void BorrowBook_WithUnavailableBook_ShouldThrowException()
        {
            var bookId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var book = new Book { Id = bookId, Title = "Test Book", AvailableCopies = 0 };

            _mockBookRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns(book);

            var act = () => _service.BorrowBook(bookId, memberId);

            act.Should().Throw<Exception>().WithMessage("*available*");
        }

        [Fact]
        public void ReturnBook_OnTime_ShouldHaveZeroFine()
        {
            var borrowingId = Guid.NewGuid();
            var borrowing = new Borrowing
            {
                Id = borrowingId,
                BookId = Guid.NewGuid(),
                DueDate = DateTime.Now.AddDays(7),
                IsReturned = false
            };
            var book = new Book { Id = borrowing.BookId, AvailableCopies = 5 };

            _mockBorrowingRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Borrowing, Borrowing>>>(),
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, IOrderedQueryable<Borrowing>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Borrowing, object>>>()
            )).Returns(borrowing);

            _mockBookRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns(book);

            _mockBookRepo.Setup(r => r.Update(It.IsAny<Book>())).Returns(book);
            _mockBorrowingRepo.Setup(r => r.Update(It.IsAny<Borrowing>())).Returns(borrowing);

            var result = _service.ReturnBook(borrowingId);

            result.Fine.Should().Be(0);
            result.IsReturned.Should().BeTrue();
        }

        [Fact]
        public void ReturnBook_Overdue_ShouldCalculateFine()
        {
            var borrowingId = Guid.NewGuid();
            var borrowing = new Borrowing
            {
                Id = borrowingId,
                BookId = Guid.NewGuid(),
                DueDate = DateTime.Now.AddDays(-5),
                IsReturned = false
            };
            var book = new Book { Id = borrowing.BookId, AvailableCopies = 5 };

            _mockBorrowingRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Borrowing, Borrowing>>>(),
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, IOrderedQueryable<Borrowing>>>(),
                It.IsAny<Func<IQueryable<Borrowing>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Borrowing, object>>>()
            )).Returns(borrowing);

            _mockBookRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Book, Book>>>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<Func<IQueryable<Book>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Book, object>>>()
            )).Returns(book);

            _mockBookRepo.Setup(r => r.Update(It.IsAny<Book>())).Returns(book);
            _mockBorrowingRepo.Setup(r => r.Update(It.IsAny<Borrowing>())).Returns(borrowing);

            var result = _service.ReturnBook(borrowingId);

            result.Fine.Should().BeGreaterThan(0);
            result.IsReturned.Should().BeTrue();
        }
    }
}