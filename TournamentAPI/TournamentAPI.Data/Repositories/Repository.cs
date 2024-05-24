using Microsoft.EntityFrameworkCore;

namespace TournamentAPI.Data.Repositories;

public abstract class Repository<TEntity> where TEntity : class, IEntity
{
    protected readonly DbSet<TEntity> _repository;

    protected Repository(DbSet<TEntity> repository)
    {
        _repository = repository;
    }

    public int Id { get; }

    public virtual async Task<IEnumerable<TEntity>?> GetAllAsync()
        => await _repository.ToListAsync();

    public virtual async Task<IEnumerable<TEntity>?> FindAsync(Func<TEntity, bool> predicate)
        => await Task.FromResult(_repository.Where(predicate));

    public virtual async Task<TEntity?> GetAsync(int id)
        => await _repository.FindAsync(id);

    public virtual async Task<bool> AnyAsync(int id)
        => await _repository.AnyAsync(e => e.Id == id);

    public virtual async Task AddAsync(TEntity entity)
        => await _repository.AddAsync(entity);

    public virtual void Update(TEntity entity)
        => _repository.Update(entity);

    public virtual void Remove(TEntity entity)
        => _repository.Remove(entity);

    public virtual void ChangeState(TEntity entity, EntityState state)
        => _repository.Update(entity).State = state;
}
