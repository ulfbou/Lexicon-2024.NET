using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;


namespace TournamentAPI.Data.Repositories;

public class TournamentRepository(TournamentContext context) : Repository<Tournament>(context.Tournament), ITournamentRepository
{
    private readonly TournamentContext _context = context;


    public async Task<IEnumerable<Tournament>?> GetAllAsync(bool include)
    {
        if (include)
        {
            return await Task.FromResult(_context.Tournament
            .Include(t => t.Games));
        }

        return await Task.FromResult(_repository);
    }

    public async Task<Tournament?> GetAsync(int id, bool include = true)
    {
        if (include)
        {
            return await _context.Tournament
                .Include(t => t.Games)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        return await _repository.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Tournament>?> FindAsync(Func<Tournament, bool> predicate, bool include)
    {
        if (include)
        {
            return await Task.FromResult(_context.Tournament
            .Include(t => t.Games)
            .Where(predicate));
        }
        return await Task.FromResult(_repository.Where(predicate));
    }

    public async Task<bool> AnyAsync(int id, bool include)
    {
        if (include)
        {
            return await _context.Tournament
                .Include(t => t.Games)
                .AnyAsync(e => e.Id == id);
        }

        return await _repository.AnyAsync(e => e.Id == id);
    }
}
