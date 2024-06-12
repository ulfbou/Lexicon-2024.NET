using Microsoft.EntityFrameworkCore;
using System.Text;
using TaskManager.Core.Models;

namespace TaskManager.Persistence.Data
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
        {
        }

        public DbSet<TaskModel> Tasks { get; set; }
    }
}
