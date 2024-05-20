using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data;

public class TournamentAPIContext : DbContext
{
    public TournamentAPIContext(DbContextOptions<TournamentAPIContext> options)
        : base(options)
    {
    }

    public DbSet<TournamentAPI.Core.Entities.Tournament> Tournament => Set<Tournament>();
    public DbSet<TournamentAPI.Core.Entities.Game> Game => Set<Game>(); 
}  