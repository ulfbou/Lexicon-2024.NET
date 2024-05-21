using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories;

// GameRepository.cs
public class GameRepository(TournamentAPIContext context) : IGameRepository
{
    private readonly TournamentAPIContext _context = context;

    public async Task<IEnumerable<Game>> GetAllAsync()
        => await _context.Game.ToListAsync();
    
    public async Task<Game?> GetAsync(int id)
        => await _context.Game.FirstOrDefaultAsync(g => g.Id == id);
    
    public async Task<bool> AnyAsync(int id)
        => await _context.Game.AnyAsync(g => g.Id == id);
    
    public void Add(Game game)
        => _context.Game.Add(game);
    
    public void Update(Game game)
        => _context.Game.Update(game);
    
    public void Remove(Game game)
        => _context.Game.Remove(game);
}
