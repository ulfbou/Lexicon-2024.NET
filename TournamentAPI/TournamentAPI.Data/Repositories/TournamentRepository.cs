using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;


namespace TournamentAPI.Data.Repositories;

public class TournamentRepository(TournamentContext context) : Repository<Tournament>(context.Tournament), ITournamentRepository
{
    private readonly TournamentContext _context = context;
    private const int pageSizeMax = 100;

    // generate implementations for the methods in ITournamentRepository
    /// <summary>
    /// Asynchronously retrieves all Tournament entities from the repository.
    /// </summary>
    /// <param name="include">A boolean value that indicates whether to include related entities in the result.</param>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Tournament entities.</returns>
    public async Task<IEnumerable<Tournament>> GetAllAsync(bool include = true, string? searchQuery = null, int pageIndex = 0, int pageSize = 10)
    {
        IQueryable<Tournament> query = _repository;

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(t => t.Title.Contains(searchQuery));
        }

        if (include)
        {
            query = query.Include(t => t.Games);
        }

        return await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a Tournament entity with the specified id from the repository.
    /// </summary>
    /// <param name="predicate">A predicate that represents the results filter.</param>
    /// <param name="include">A boolean value that indicates whether to include related entities in the result.</param>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <pagam name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Tournament entities that satisfy the predicate.</returns>
    public async Task<IEnumerable<Tournament>> FindAsync(Expression<Func<Tournament, bool>>? predicate, bool include = true, string? searchQuery = null, int pageIndex = 0, int pageSize = 10)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        if (pageSize > pageSizeMax) pageSize = pageSizeMax;

        IQueryable<Tournament> query = _repository;

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (include)
        {
            query = query.Include(t => t.Games);
        }

        return await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a Tournament entity with the specified id from the repository.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="include">A boolean value that indicates whether to include related entities in the result.</param>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Tournament object if found, null otherwise.</returns>
    public async Task<Tournament?> GetAsync(int id, bool include = true)
    {
        IQueryable<Tournament> query = _repository.Where(t => t.Id == id);

        if (include && query.Count() > 0)
        {
            query = query.Include(t => t.Games);
        }

        return await query.FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Gets games from the tournament with the specified tournament id.
    /// </summary>
    /// <param name="tournamentId">An identifier for the specified tournament.</param>
    /// <param name="title">A string that represents the title of the game to filter the results.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Game objects if found, null otherwise.</returns>
    public async Task<IEnumerable<Game>?> GetGamesAsync(int tournamentId, string? title = null, int pageIndex = 0, int pageSize = 10)
    {
        var tournament = await _repository.FindAsync(tournamentId, title, pageIndex, pageSize);

        if (tournament is null)
        {
            return null;
        }

        IQueryable<Game> query = _context.Entry(tournament)
                .Collection(t => t.Games)
                .Query();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(g => g.Title.Contains(title));
        }

        return await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }



    /// <summary>
    /// Gets the game with the specified game id from the tournament with the specified tournament id.
    /// </summary>
    /// <param name="tournamentId">An identifier for the specified tournament.</param>
    /// <param name="gameId">An identifier for the specified game.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Game object if found, null otherwise.</returns>
    public async Task<Game?> GetGameAsync(int tournamentId, int gameId)
    {
        var tournament = await _repository.FindAsync(tournamentId);

        if (tournament is null)
        {
            return null;
        }

        return await _context.Entry(tournament)
            .Collection(t => t.Games)
            .Query()
            .FirstOrDefaultAsync(g => g.Id == gameId);
    }
}
