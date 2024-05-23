using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;


namespace TournamentAPI.Data.Repositories;

public partial class TournamentRepository(TournamentContext context) : ITournamentRepository
{
    private readonly TournamentContext _context = context;

    public int Id => throw new NotImplementedException();

    public async Task<IEnumerable<Tournament>> GetAllAsync()
        => await _context.Tournament.Include(t => t.Games).ToListAsync();
    
    public async Task<Tournament?> GetAsync(int id)
    {
        return await _context.Tournament.Include(t => t.Games).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> AnyAsync(int id)
        => await GetAsync(id) != null;
    
    public void Add(Tournament tournament)
        => _context.Tournament.Add(tournament);
    
    public void Update(Tournament tournament)
        => _context.Tournament.Update(tournament);
    
    public void Remove(Tournament tournament)
        => _context.Tournament.Remove(tournament);

    public void ChangeState(Tournament tournament, EntityState state)
        => _context.Entry(tournament).State = state;
}
