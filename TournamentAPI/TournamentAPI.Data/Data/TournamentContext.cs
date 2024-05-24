using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data;

public class TournamentContext : DbContext
{
    public TournamentContext(DbContextOptions<TournamentContext> options)
        : base(options)
    {
    }

    public DbSet<Tournament> Tournament => Set<Tournament>();
    public DbSet<Game> Game => Set<Game>();
}
