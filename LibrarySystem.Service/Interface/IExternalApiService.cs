using LibrarySystem.Domain.DTOs;

namespace LibrarySystem.Service.Interface
{
    public interface IExternalApiService
    {
        Task<List<OpenLibraryBookDTO>> SearchBooksAsync(string query);
        Task<OpenLibraryBookDTO?> GetBookByISBNAsync(string isbn);
    }
}