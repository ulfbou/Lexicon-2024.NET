using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskManager.Core.Models;
using TaskManager.Persistence.Data;
using Zentient.Repository;

namespace TaskManager.Persistence.Services
{
    public class TaskServices : ITaskServices
    {
        private readonly TaskManagerContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private IRepository<TaskModel, int> _taskRepository;

        public TaskServices(TaskManagerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _unitOfWork = new UnitOfWork(context) ?? throw new InvalidOperationException("Unable to create a unit of work");
            _taskRepository = _unitOfWork.GetRepository<TaskModel, int>() ?? throw new InvalidOperationException("Unable to create a repository");
        }

        public async Task<IEnumerable<TaskModel>> GetTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskModel> GetTaskAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskModel?> CreateOrUpdateTaskAsync(TaskModel task)
        {
            var taskModel = await _taskRepository.GetAsync(task.Id);
            EntityEntry? entityEntry = null;

            if (taskModel is null)
            {
                entityEntry = await _taskRepository.AddAsync(task);
            }
            else if (task.Id != taskModel.Id)
            {
                throw new InvalidCastException($"The task id {task.Id} does not match the task id {taskModel.Id}");
            }
            else
            {
                entityEntry = await _taskRepository.UpdateAsync(task);
            }

            return entityEntry?.Entity is TaskModel createdTask ? createdTask : null;
        }

        public async Task UpdateTaskAsync(TaskModel task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

    }
}