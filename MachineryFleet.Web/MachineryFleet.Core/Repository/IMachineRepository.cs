using MachineryFleet.Core.Entities;
using Zentient.Repository;

namespace MachineryFleet.Core.Repository
{
    public interface IMachineRepository : IRepository<Machine, Guid>
    {
    }
}
