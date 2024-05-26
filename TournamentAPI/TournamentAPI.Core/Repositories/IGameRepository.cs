using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Repositories;

public interface IGameRepository : IRepository<Game>
{
}
