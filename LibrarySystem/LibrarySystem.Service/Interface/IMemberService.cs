using LibrarySystem.Domain.Models;

namespace LibrarySystem.Service.Interface
{
    public interface IMemberService
    {
        List<Member> GetAllMembers();
        Member? GetMemberById(Guid id);
        void CreateMember(Member member);
        void UpdateMember(Member member);
        void DeleteMember(Guid id);
    }
}