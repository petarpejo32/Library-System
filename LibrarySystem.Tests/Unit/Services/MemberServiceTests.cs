using Xunit;
using Moq;
using LibrarySystem.Service.Implementation;
using LibrarySystem.Repository.Interface;
using LibrarySystem.Domain.Models;
using FluentAssertions;
using System.Linq.Expressions;

namespace LibrarySystem.Tests.Unit.Services
{
    public class MemberServiceTests
    {
        private readonly Mock<IRepository<Member>> _mockRepo;
        private readonly MemberService _service;

        public MemberServiceTests()
        {
            _mockRepo = new Mock<IRepository<Member>>();
            _service = new MemberService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllMembers_ShouldReturnAllMembers()
        {
            var members = new List<Member>
            {
                new Member { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@test.com" },
                new Member { Id = Guid.NewGuid(), Name = "Jane Doe", Email = "jane@test.com" }
            };

            _mockRepo.Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Member, Member>>>(),
                It.IsAny<Expression<Func<Member, bool>>>(),
                It.IsAny<Func<IQueryable<Member>, IOrderedQueryable<Member>>>(),
                It.IsAny<Func<IQueryable<Member>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Member, object>>>()
            )).Returns(members);

            var result = _service.GetAllMembers();

            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetMemberById_WhenExists_ShouldReturnMember()
        {
            var id = Guid.NewGuid();
            var member = new Member { Id = id, Name = "Test Member", Email = "test@test.com" };

            _mockRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Member, Member>>>(),
                It.IsAny<Expression<Func<Member, bool>>>(),
                It.IsAny<Func<IQueryable<Member>, IOrderedQueryable<Member>>>(),
                It.IsAny<Func<IQueryable<Member>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Member, object>>>()
            )).Returns(member);

            var result = _service.GetMemberById(id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Member");
        }

        [Fact]
        public void GetMemberById_WhenNotExists_ShouldReturnNull()
        {
            _mockRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Member, Member>>>(),
                It.IsAny<Expression<Func<Member, bool>>>(),
                It.IsAny<Func<IQueryable<Member>, IOrderedQueryable<Member>>>(),
                It.IsAny<Func<IQueryable<Member>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Member, object>>>()
            )).Returns((Member?)null);

            var result = _service.GetMemberById(Guid.NewGuid());

            result.Should().BeNull();
        }

        [Fact]
        public void CreateMember_ShouldGenerateIdAndCallInsert()
        {
            var member = new Member { Name = "New Member", Email = "new@test.com" };
            _mockRepo.Setup(r => r.Insert(It.IsAny<Member>())).Returns(member);

            _service.CreateMember(member);

            member.Id.Should().NotBe(Guid.Empty);
            _mockRepo.Verify(r => r.Insert(It.IsAny<Member>()), Times.Once);
        }

        [Fact]
        public void UpdateMember_ShouldCallUpdate()
        {
            var member = new Member { Id = Guid.NewGuid(), Name = "Updated Member" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Member>())).Returns(member);

            _service.UpdateMember(member);

            _mockRepo.Verify(r => r.Update(It.Is<Member>(m => m.Id == member.Id)), Times.Once);
        }

        [Fact]
        public void DeleteMember_ShouldCallDelete()
        {
            var id = Guid.NewGuid();
            var member = new Member { Id = id, Name = "To Delete" };

            _mockRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<Member, Member>>>(),
                It.IsAny<Expression<Func<Member, bool>>>(),
                It.IsAny<Func<IQueryable<Member>, IOrderedQueryable<Member>>>(),
                It.IsAny<Func<IQueryable<Member>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Member, object>>>()
            )).Returns(member);

            _mockRepo.Setup(r => r.Delete(It.IsAny<Member>())).Returns(member);

            _service.DeleteMember(id);

            _mockRepo.Verify(r => r.Delete(It.IsAny<Member>()), Times.Once);
        }
    }
}