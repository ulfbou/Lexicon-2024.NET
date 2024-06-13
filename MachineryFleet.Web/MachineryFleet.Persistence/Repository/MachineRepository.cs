using MachineryFleet.Core.Entities;
using MachineryFleet.Core.Repository;
using MachineryFleet.Persistence.Data;
using Zentient.Repository;

namespace MachineryFleet.Persistence.Repository
{
    public class MachineRepository : RepositoryBase<Machine, Guid>, IMachineRepository
    {
        public MachineRepository(MachineryFleetDbContext context) : base(context)
        {
        }
    }
}

