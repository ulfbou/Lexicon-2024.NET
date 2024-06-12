using TaskManager.Core.Models;

namespace TaskManager.Persistence.Services
{
    public interface ITaskService
    {
        Task AddTaskAsync(TaskModel task);
        Task DeleteTaskAsync(int id);
        Task<TaskModel> GetTaskAsync(int id);
        Task<List<TaskModel>> GetTasksAsync();
        Task UpdateTaskAsync(TaskModel task);
    }
}