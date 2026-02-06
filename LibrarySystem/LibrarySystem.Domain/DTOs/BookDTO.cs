namespace LibrarySystem.Domain.DTOs
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PublishedYear { get; set; }
        public string? Genre { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? AuthorName { get; set; }
        public Guid AuthorId { get; set; }
    }
}