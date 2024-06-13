using MachineryFleet.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MachineryFleet.Persistence.Data
{
    public class MachineryFleetDbContext : DbContext
    {
        public MachineryFleetDbContext(DbContextOptions<MachineryFleetDbContext> options) : base(options)
        {
        }

        public DbSet<Machine> Machines => Set<Machine>();
    }
}
