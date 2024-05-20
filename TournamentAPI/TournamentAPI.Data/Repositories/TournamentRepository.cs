using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;
using TournamentAPI.Data.Data;


namespace TournamentAPI.Data.Repositories;

public class TournamentRepository(TournamentAPI.Data.Data.TournamentAPIContext context) : ITournamentRepository
{
    private readonly TournamentAPIContext _context = context;

    public async Task<IEnumerable<Tournament>> GetAllAsync()
        => await _context.Tournament.Include(t => t.Games).ToListAsync();
    
    public async Task<Tournament?> GetAsync(int id)
        => await _context.Tournament.Include(t => t.Games).FirstOrDefaultAsync(t => t.Id == id);
    
    public async Task<bool> AnyAsync(int id)
        => await GetAsync(id) != null;
    
    public void Add(Tournament tournament)
        => _context.Tournament.Add(tournament);
    
    public void Update(Tournament tournament)
        => _context.Tournament.Update(tournament);
    

    public void Remove(Tournament tournament)
        => _context.Tournament.Remove(tournament);
}
