using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Repositories;

public interface ITournamentRepository : IRepository<Tournament>
{
    /// <summary>
    /// Asynchronously retrieves all Tournament entities from the repository.
    /// </summary>
    /// <param name="include">A boolean value that indicates whether to include related entities in the result.</param>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Tournament entities.</returns>
    public Task<IEnumerable<Tournament>> GetAllAsync(bool include = true, string? searchQuery = null, int pageIndex = 0, int pageSize = 10);

    /// <summary>
    /// Asynchronously retrieves a Tournament entity with the specified id from the repository.
    /// </summary>
    /// <param name="predicate">A predicate that represents the results filter.</param>
    /// <param name="include">A boolean value that indicates whether to include related entities in the result.</param>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <pagam name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Tournament entities that satisfy the predicate.</returns>
    public Task<IEnumerable<Tournament>> FindAsync(Expression<Func<Tournament, bool>>? predicate, bool include = true, string? searchQuery = null, int pageIndex = 0, int pageSize = 10);

    /// <summary>
    /// Asynchronously retrieves a Tournament entity with the specified id from the repository.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="include">A boolean value that indicates whether to include related entities in the result.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Tournament object if found, null otherwise.</returns>
    public Task<Tournament?> GetAsync(int id, bool include = true);

    /// <summary>
    /// Gets the game with the specified game id from the tournament with the specified tournament id.
    /// </summary>
    /// <param name="tournamentId">An identifier for the specified tournament.</param>
    /// <param name="gameId">An identifier for the specified game.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Game object if found, null otherwise.</returns>
    public Task<Game?> GetGameAsync(int tournamentId, int gameId);
    Task<IEnumerable<Game>?> GetGamesAsync(int tournamentId, string? title = null, int pageIndex = 0, int pageSize = 10);
}
