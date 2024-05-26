using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories;

// GameRepository.cs
public class GameRepository(TournamentContext context) : Repository<Game>(context.Game), IGameRepository
{
}
