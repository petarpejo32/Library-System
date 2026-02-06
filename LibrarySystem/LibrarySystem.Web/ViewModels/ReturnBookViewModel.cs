using LibrarySystem.Domain.Models;

namespace LibrarySystem.Web.ViewModels
{
    public class ReturnBookViewModel
    {
        public Borrowing Borrowing { get; set; }
        public int DaysUntilDue { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysOverdue { get; set; }
        public decimal EstimatedFine { get; set; }
        public string BookTitle { get; set; }
        public string AuthorName { get; set; }
        public string MemberName { get; set; }
    }
}