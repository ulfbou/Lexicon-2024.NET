using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Repositories;

/// <summary>
/// Provides an interface for a repository that handles operations for Game entities.
/// </summary>
/// <remarks>
/// This interface inherits methods from IRepository<Game>:
/// <list type="bullet">
/// <item>
/// <description>GetAllAsync: Asynchronously retrieves all Game entities from the repository.</description>
/// </item>
/// <item>
/// <description>GetAsync: Asynchronously retrieves a Game entity with the specified id from the repository.</description>
/// </item>
/// <item>
/// <description>FindAsync: Asynchronously retrieves all Game entities that satisfy the specified predicate.</description>
/// </item>
/// <item>
/// <description>AnyAsync: Asynchronously determines whether a Game entity with the specified id exists in the repository.</description>
/// </item>
/// <item>
/// <description>Add: Adds the specified Game entity to the repository.</description>
/// </item>
/// <item>
/// <description>Update: Updates the specified Game entity in the repository.</description>
/// </item>
/// <item>
/// <description>Remove: Removes the specified Game entity from the repository.</description>
/// </item>
/// <item>
/// <description>ChangeState: Changes the state of the specified Game entity in the repository.</description>
/// </item>
/// </list>
/// </remarks>
public interface IGameRepository : IRepository<Game>
{
}