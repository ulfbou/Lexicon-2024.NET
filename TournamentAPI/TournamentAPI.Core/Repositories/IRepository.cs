using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(int id);
    Task<bool> AnyAsync(int id);
    void Add(T t);
    void Update(T t);
    void Remove(T t);
    void ChangeState(T t, EntityState state);
}