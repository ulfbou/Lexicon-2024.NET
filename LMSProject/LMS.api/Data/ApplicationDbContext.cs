using LMS.api.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.api.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        public DbSet<Activity> Activity => Set<Activity>();
        public DbSet<Course> Course => Set<Course>();
        public DbSet<Document> Document => Set<Document>();
        public DbSet<Module> Module => Set<Module>();
        public DbSet<Student> Student => Set<Student>();
        public DbSet<Teacher> Teacher => Set<Teacher>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
