using LibrarySystem.Domain.DTOs;
using LibrarySystem.Service.Interface;
using System.Text.Json;

namespace LibrarySystem.Service.Implementation
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://openlibrary.org/search.json";

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OpenLibraryBookDTO>> SearchBooksAsync(string query)
        {
            try
            {
                var url = $"{BASE_URL}?q={Uri.EscapeDataString(query)}&limit=10";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<OpenLibraryBookDTO>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<OpenLibraryApiResponseDTO>(content);

                return apiResponse?.Books ?? new List<OpenLibraryBookDTO>();
            }
            catch (Exception)
            {
                return new List<OpenLibraryBookDTO>();
            }
        }

        public async Task<OpenLibraryBookDTO?> GetBookByISBNAsync(string isbn)
        {
            try
            {
                var url = $"{BASE_URL}?isbn={Uri.EscapeDataString(isbn)}&limit=1";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<OpenLibraryApiResponseDTO>(content);

                return apiResponse?.Books?.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}