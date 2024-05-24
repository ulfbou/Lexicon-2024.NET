using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories;

// GameRepository.cs
public class GameRepository(TournamentContext context) : Repository<Game>(context.Game), IGameRepository
{
}
