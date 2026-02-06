using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Models
{
    public class Borrowing : BaseEntity
    {
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        [Range(0, 10000)]
        public decimal Fine { get; set; }

        public Guid BookId { get; set; }
        public virtual Book? Book { get; set; }

        public Guid MemberId { get; set; }
        public virtual Member? Member { get; set; }
    }
}