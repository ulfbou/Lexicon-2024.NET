using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;


namespace TournamentAPI.Data.Repositories;

public class TournamentRepository(TournamentContext context) : Repository<Tournament>(context.Tournament), ITournamentRepository
{
    private readonly TournamentContext _context = context;

    public virtual async Task<Tournament?> GetAsync(int id, bool include = true)
    {
        if (include)
        {
            return await _context.Tournament
                .Include(t => t.Games)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        return await _context.Tournament.FirstOrDefaultAsync(t => t.Id == id);
    }

    public override async Task<IEnumerable<Tournament>?> GetAllAsync()
        => await Task.FromResult(_context.Tournament
            .Include(t => t.Games));

    public override async Task<IEnumerable<Tournament>?> FindAsync(Func<Tournament, bool> predicate)
        => await Task.FromResult(_context.Tournament
            .Include(t => t.Games)
            .Where(predicate));

    public override async Task<bool> AnyAsync(int id)
        => await _context.Tournament
            .Include(t => t.Games)
            .AnyAsync(e => e.Id == id);
}
