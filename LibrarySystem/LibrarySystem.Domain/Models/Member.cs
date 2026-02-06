using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Models
{
    public class Member : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public DateTime MembershipDate { get; set; }

        public virtual ICollection<Borrowing>? Borrowings { get; set; }
    }
}