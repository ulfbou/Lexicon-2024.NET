
namespace TaskManager.Persistence.Services
{
    public interface IApiService
    {
        Task DeleteAsync(string uri);
        Task<T> GetAsync<T>(string uri);
        Task PostAsync<T>(string uri, T content);
        Task PutAsync<T>(string uri, T content);
    }
}