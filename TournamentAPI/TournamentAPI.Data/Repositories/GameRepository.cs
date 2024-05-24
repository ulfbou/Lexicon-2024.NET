using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories;

// GameRepository.cs
public class GameRepository : Repository<Game>, IGameRepository
{
    public GameRepository(TournamentContext context) : base(context.Game)
    {
    }
}
