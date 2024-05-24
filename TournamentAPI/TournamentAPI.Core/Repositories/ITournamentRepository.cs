using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Repositories;

/// <summary>
/// Provides an interface for a repository that handles operations for Tournament entities.
/// </summary>
/// <remarks>
/// This interface inherits methods from IRepository<Tournament>:
/// <list type="bullet">
/// <item>
/// <description>GetAllAsync: Asynchronously retrieves all Tournament entities from the repository.</description>
/// </item>
/// <item>
/// <description>GetAsync: Asynchronously retrieves a Tournament entity with the specified id from the repository.</description>
/// </item>
/// <item>
/// <description>FindAsync: Asynchronously retrieves all Tournament entities that satisfy the specified predicate.</description>
/// </item>
/// <item>
/// <description>AnyAsync: Asynchronously determines whether a Tournament entity with the specified id exists in the repository.</description>
/// </item>
/// <item>
/// <description>Add: Adds the specified Tournament entity to the repository.</description>
/// </item>
/// <item>
/// <description>Update: Updates the specified Tournament entity in the repository.</description>
/// </item>
/// <item>
/// <description>Remove: Removes the specified Tournament entity from the repository.</description>
/// </item>
/// <item>
/// <description>ChangeState: Changes the state of the specified Tournament entity in the repository.</description>
/// </item>
/// </list>
/// </remarks>

public interface ITournamentRepository : IRepository<Tournament>
{
    public Task<IEnumerable<Tournament>?> GetAllAsync(bool inclusion);

    public Task<IEnumerable<Tournament>?> FindAsync(Func<Tournament, bool> predicate, bool include = true);
    public Task<Tournament?> GetAsync(int id, bool include = true);
}
