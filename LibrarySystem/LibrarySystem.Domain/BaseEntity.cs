using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}