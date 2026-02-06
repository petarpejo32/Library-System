using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Models
{
    public class Book : BaseEntity  
    {

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; }

        public int PublishedYear { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public Guid AuthorId { get; set; }
        public virtual Author? Author { get; set; }

        public virtual ICollection<Borrowing>? Borrowings { get; set; }
    }
}