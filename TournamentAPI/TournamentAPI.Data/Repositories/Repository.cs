using Microsoft.EntityFrameworkCore;

namespace TournamentAPI.Data.Repositories;

public abstract class Repository<TEntity> where TEntity : class, IEntity
{
    protected readonly DbSet<TEntity> _repository = null!;

    protected Repository(DbSet<TEntity> repository)
    {
        _repository = repository;
    }

    public int Id { get; }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _repository.ToListAsync();

    public async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
        => await Task.FromResult(_repository.Where(predicate));

    public async Task<TEntity?> GetAsync(int id)
        => await _repository.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<bool> AnyAsync(int id)
        => await _repository.AnyAsync(e => e.Id == id);

    public async Task AddAsync(TEntity entity)
        => await _repository.AddAsync(entity);

    public void Update(TEntity entity)
        => _repository.Update(entity);

    public void Remove(TEntity entity)
        => _repository.Remove(entity);

    public void ChangeState(TEntity entity, EntityState state)
        => _repository.Update(entity).State = state;
}
