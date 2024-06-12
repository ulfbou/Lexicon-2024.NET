using System.Net.Http.Json;
using TaskManager.Core.Models;

namespace TaskManager.Persistence.Services
{
    public class TaskService : ITaskService
    {
        private readonly HttpClient _httpClient;

        public TaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TaskModel>> GetTasksAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskModel>>("api/tasks");
        }

        public async Task<TaskModel> GetTaskAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TaskModel>($"api/tasks/{id}");
        }

        public async Task AddTaskAsync(TaskModel task)
        {
            await _httpClient.PostAsJsonAsync("api/tasks", task);
        }

        public async Task UpdateTaskAsync(TaskModel task)
        {
            await _httpClient.PutAsJsonAsync($"api/tasks/{task.Id}", task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/tasks/{id}");
        }
    }
}