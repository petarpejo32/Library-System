using LibrarySystem.Domain.Models;
using LibrarySystem.Repository.Interface;
using LibrarySystem.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Service.Implementation
{
    public class MemberService : IMemberService
    {
        private readonly IRepository<Member> _memberRepository;

        public MemberService(IRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public List<Member> GetAllMembers()
        {
            return _memberRepository.GetAll(
                member => member,
                include: query => query.Include(m => m.Borrowings)
            ).ToList();
        }

        public Member? GetMemberById(Guid id)
        {
            return _memberRepository.Get(
                member => member,
                predicate: m => m.Id == id,
                include: query => query.Include(m => m.Borrowings)
            );
        }

        public void CreateMember(Member member)
        {
            member.Id = Guid.NewGuid();
            member.MembershipDate = DateTime.Now;
            _memberRepository.Insert(member);
        }

        public void UpdateMember(Member member)
        {
            _memberRepository.Update(member);
        }

        public void DeleteMember(Guid id)
        {
            var member = GetMemberById(id);
            if (member != null)
            {
                _memberRepository.Delete(member);
            }
        }
    }
}