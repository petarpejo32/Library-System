using LibrarySystem.Domain.Models;

namespace LibrarySystem.Service.Interface
{
    public interface IBorrowingService
    {
        List<Borrowing> GetAllBorrowings();
        Borrowing? GetBorrowingById(Guid id);
        void CreateBorrowing(Borrowing borrowing);
        void UpdateBorrowing(Borrowing borrowing);
        void DeleteBorrowing(Guid id);

        Borrowing BorrowBook(Guid bookId, Guid memberId);
        Borrowing ReturnBook(Guid borrowingId);

        List<Borrowing> GetActiveBorrowings();
        List<Borrowing> GetOverdueBorrowings();
        List<Borrowing> GetBorrowingsByMember(Guid memberId);
        List<Borrowing> GetBorrowingsByBook(Guid bookId);
    }
}