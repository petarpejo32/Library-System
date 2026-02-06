using LibrarySystem.Domain.Models;
using LibrarySystem.Repository.Interface;
using LibrarySystem.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Service.Implementation
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IRepository<Borrowing> _borrowingRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Member> _memberRepository;

        public BorrowingService(
            IRepository<Borrowing> borrowingRepository,
            IRepository<Book> bookRepository,
            IRepository<Member> memberRepository)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
        }

        #region Basic CRUD Operations

        public List<Borrowing> GetAllBorrowings()
        {
            return _borrowingRepository.GetAll(
                borrowing => borrowing,
                include: query => query
                    .Include(b => b.Book)
                        .ThenInclude(book => book.Author)
                    .Include(b => b.Member)
            ).ToList();
        }

        public Borrowing? GetBorrowingById(Guid id)
        {
            return _borrowingRepository.Get(
                borrowing => borrowing,
                predicate: b => b.Id == id,
                include: query => query
                    .Include(b => b.Book)
                        .ThenInclude(book => book.Author)
                    .Include(b => b.Member)
            );
        }

        public void CreateBorrowing(Borrowing borrowing)
        {
            borrowing.Id = Guid.NewGuid();
            _borrowingRepository.Insert(borrowing);
        }

        public void UpdateBorrowing(Borrowing borrowing)
        {
            _borrowingRepository.Update(borrowing);
        }

        public void DeleteBorrowing(Guid id)
        {
            var borrowing = GetBorrowingById(id);
            if (borrowing != null)
            {
                if (!borrowing.IsReturned)
                {
                    var book = _bookRepository.Get(
                        b => b,
                        predicate: b => b.Id == borrowing.BookId
                    );

                    if (book != null)
                    {
                        book.AvailableCopies++;
                        _bookRepository.Update(book);
                    }
                }
                
                _borrowingRepository.Delete(borrowing);
            }
        }


        public Borrowing BorrowBook(Guid bookId, Guid memberId)
        {
            var book = _bookRepository.Get(
                b => b,
                predicate: b => b.Id == bookId,
                include: query => query.Include(b => b.Author)
            );

            if (book == null)
                throw new Exception("Book not found!");

            if (book.AvailableCopies <= 0)
                throw new Exception($"No copies of '{book.Title}' are currently available!");

            var member = _memberRepository.Get(
                m => m,
                predicate: m => m.Id == memberId
            );

            if (member == null)
                throw new Exception("Member not found!");

            var existingBorrowing = _borrowingRepository.Get(
                b => b,
                predicate: b => b.MemberId == memberId &&
                               b.BookId == bookId &&
                               !b.IsReturned
            );

            if (existingBorrowing != null)
                throw new Exception($"Member already has an active borrowing of '{book.Title}'!");

            var borrowing = new Borrowing
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                MemberId = memberId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14), 
                IsReturned = false,
                Fine = 0
            };

            book.AvailableCopies--;
            _bookRepository.Update(book);

            _borrowingRepository.Insert(borrowing);

            return borrowing;
        }

        public Borrowing ReturnBook(Guid borrowingId)
        {
            var borrowing = _borrowingRepository.Get(
                b => b,
                predicate: b => b.Id == borrowingId,
                include: query => query
                    .Include(b => b.Book)
                        .ThenInclude(book => book.Author)
                    .Include(b => b.Member)
            );

            if (borrowing == null)
                throw new Exception("Borrowing record not found!");

            if (borrowing.IsReturned)
                throw new Exception("This book has already been returned!");

            borrowing.ReturnDate = DateTime.Now;
            borrowing.IsReturned = true;

            if (DateTime.Now > borrowing.DueDate)
            {
                var daysOverdue = (DateTime.Now - borrowing.DueDate).Days;
                borrowing.Fine = daysOverdue * 1.0m;
            }

            var book = _bookRepository.Get(
                b => b,
                predicate: b => b.Id == borrowing.BookId
            );

            if (book != null)
            {
                book.AvailableCopies++;
                _bookRepository.Update(book);
            }

            _borrowingRepository.Update(borrowing);

            return borrowing;
        }

        public List<Borrowing> GetActiveBorrowings()
        {
            return _borrowingRepository.GetAll(
                borrowing => borrowing,
                predicate: b => !b.IsReturned,
                include: query => query
                    .Include(b => b.Book)
                        .ThenInclude(book => book.Author)
                    .Include(b => b.Member)
            ).ToList();
        }

        public List<Borrowing> GetOverdueBorrowings()
        {
            return _borrowingRepository.GetAll(
                borrowing => borrowing,
                predicate: b => !b.IsReturned && b.DueDate < DateTime.Now,
                include: query => query
                    .Include(b => b.Book)
                        .ThenInclude(book => book.Author)
                    .Include(b => b.Member),
                orderBy: query => query.OrderBy(b => b.DueDate)
            ).ToList();
        }

        public List<Borrowing> GetBorrowingsByMember(Guid memberId)
        {
            return _borrowingRepository.GetAll(
                borrowing => borrowing,
                predicate: b => b.MemberId == memberId,
                include: query => query
                    .Include(b => b.Book)
                        .ThenInclude(book => book.Author)
                    .Include(b => b.Member),
                orderBy: query => query.OrderByDescending(b => b.BorrowDate)
            ).ToList();
        }

        public List<Borrowing> GetBorrowingsByBook(Guid bookId)
        {
            return _borrowingRepository.GetAll(
                borrowing => borrowing,
                predicate: b => b.BookId == bookId,
                include: query => query
                    .Include(b => b.Book)
                        .ThenInclude(book => book.Author)
                    .Include(b => b.Member),
                orderBy: query => query.OrderByDescending(b => b.BorrowDate)
            ).ToList();
        }

        #endregion
    }
}