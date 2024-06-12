using System.Net.Http.Json;

namespace TaskManager.Persistence.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            return await _httpClient.GetFromJsonAsync<T>(uri);
        }

        public async Task PostAsync<T>(string uri, T content)
        {
            await _httpClient.PostAsJsonAsync(uri, content);
        }

        public async Task PutAsync<T>(string uri, T content)
        {
            await _httpClient.PutAsJsonAsync(uri, content);
        }

        public async Task DeleteAsync(string uri)
        {
            await _httpClient.DeleteAsync(uri);
        }
    }
}