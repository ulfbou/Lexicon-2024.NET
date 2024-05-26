using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TournamentAPI.Data.Repositories;

public abstract class Repository<TEntity> where TEntity : class, IEntity
{
    protected readonly DbSet<TEntity> _repository;
    private const int pageSizeMax = 100;

    protected Repository(DbSet<TEntity> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity from the repository.
    /// </summary>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity.</returns>
    public virtual async Task<IEnumerable<TEntity>?> GetAllAsync(
        string? searchQuery,
        int pageIndex = 0,
        int pageSize = 10)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        if (pageSize > pageSizeMax) pageSize = pageSizeMax;

        IQueryable<TEntity> query = _repository;

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(e => e.Title.Contains(searchQuery));
        }

        return await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity that satisfy the predicate.</returns>
    public virtual async Task<IEnumerable<TEntity>?> FindAsync(Expression<Func<TEntity, bool>>? predicate = null, int pageIndex = 0, int pageSize = 10)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        if (pageSize > pageSizeMax) pageSize = pageSizeMax;

        if (predicate is null)
        {
            return await _repository
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        return await _repository
            .Where(predicate)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously determines whether an entity of type TEntity with the specified id exists in the repository.
    /// </summary>
    /// <param name="id">The id of the entity to locate in the repository.</param>
    /// <pagam name="include">A boolean value that indicates whether to include related entities in the result.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if an entity with the specified id exists, false otherwise.</returns>
    public virtual async Task<TEntity?> GetAsync(int id)
        => await _repository.Where(e => e.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Asynchronously determines whether an entity of type TEntity with the specified id exists in the repository.
    /// </summary>
    /// <param name="id">The id of the entity to locate in the repository.</param>
    /// <pagam name="include">A boolean value that indicates whether to include related entities in the result.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if an entity with the specified id exists, false otherwise.</returns>
    public virtual async Task<bool> AnyAsync(int id)
        => await _repository.AnyAsync(e => e.Id == id);

    /// <summary>
    /// Adds the specified entity of type TEntity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add to the repository.</param>
    public virtual async Task AddAsync(TEntity entity)
        => await _repository.AddAsync(entity);

    /// <summary>
    /// Updates the specified entity of type TEntity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update in the repository.</param>
    public virtual void Update(TEntity entity)
        => _repository.Update(entity);

    /// <summary>
    /// Removes the specified entity of type TEntity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove from the repository.</param>
    public virtual void Remove(TEntity entity)
        => _repository.Remove(entity);

    /// <summary>
    /// Changes the state of the specified entity of type TEntity in the repository.
    /// </summary>
    /// <param name="entity">The entity whose state is to be changed.</param>
    /// <param name="state">The new state for the entity.</param>
    public virtual void ChangeState(TEntity entity, EntityState state)
        => _repository.Entry(entity).State = state;
}
