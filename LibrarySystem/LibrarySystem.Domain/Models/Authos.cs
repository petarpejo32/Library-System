using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Models
{
    public class Author : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Biography { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        public int? BirthYear { get; set; }

        public virtual ICollection<Book>? Books { get; set; }
    }
}