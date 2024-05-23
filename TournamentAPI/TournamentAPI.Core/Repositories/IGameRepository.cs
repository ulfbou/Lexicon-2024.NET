using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Repositories;

public interface IGameRepository : IRepository<Game>
{
}
