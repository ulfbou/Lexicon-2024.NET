using TournamentAPI.Core.Entities;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories;

public interface ITournamentRepository
{
    Task<IEnumerable<Tournament>> GetAllAsync();
    Task<Tournament?> GetAsync(int id);
    Task<bool> AnyAsync(int id);
    void Add(Tournament tournament);
    void Update(Tournament tournament);
    void Remove(Tournament tournament);
}
