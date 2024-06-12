using MessengerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Data
{
    public class MessangerContext : DbContext
    {
        public MessangerContext(DbContextOptions<MessangerContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<Message>(Message => Message.Messages)
                .WithOne(User => User.Sender)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
