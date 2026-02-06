using System.Text.Json.Serialization;

namespace LibrarySystem.Domain.DTOs
{
    public class OpenLibraryApiResponseDTO
    {
        [JsonPropertyName("docs")]
        public List<OpenLibraryBookDTO> Books { get; set; } = new();
    }

    public class OpenLibraryBookDTO
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("author_name")]
        public List<string>? AuthorNames { get; set; }

        [JsonPropertyName("isbn")]
        public List<string>? ISBNs { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonPropertyName("subject")]
        public List<string>? Subjects { get; set; }

        [JsonPropertyName("cover_i")]
        public int? CoverId { get; set; }

        public string AuthorName => AuthorNames?.FirstOrDefault() ?? "Unknown Author";

        public string ISBN => ISBNs?.FirstOrDefault() ?? "N/A";

        public string Genre => Subjects?.FirstOrDefault() ?? "Unknown";
    }
}