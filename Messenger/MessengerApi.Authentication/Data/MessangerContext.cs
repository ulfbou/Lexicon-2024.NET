using MessengerAPI.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Authentication.Data
{
    public class MessangerContext : DbContext
    {
        public MessangerContext(DbContextOptions<MessangerContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<Message>(Message => Message.Messages)
                .WithOne(User => User.Sender)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
