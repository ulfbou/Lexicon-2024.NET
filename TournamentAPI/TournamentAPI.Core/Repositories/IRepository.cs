using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TournamentAPI.Core.Entities;

public interface IRepository<TEntity> where TEntity : IEntity
{
    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity from the repository.
    /// </summary>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity.</returns>
    Task<IEnumerable<TEntity>?> GetAllAsync(string? searchQuery = null, int pageIndex = 0, int pageSize = 10);

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity that satisfy the predicate.</returns>
    Task<IEnumerable<TEntity>?> FindAsync(Expression<Func<TEntity, bool>>? predicate = null, int pageIndex = 0, int pageSize = 10);

    /// <summary>
    /// Asynchronously retrieves an entity of type TEntity with the specified id from the repository.
    /// </summary>
    /// <param name="id">The id of the entity to retrieve.</param>
    /// <param name="searchQuery">A string that represents the search query to filter the results.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the TEntity object if found, null otherwise.</returns>
    Task<TEntity?> GetAsync(int id);

    /// <summary>
    /// Asynchronously determines whether an entity of type TEntity with the specified id exists in the repository.
    /// </summary>
    /// <param name="id">The id of the entity to locate in the repository.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if an entity with the specified id exists, false otherwise.</returns>
    Task<bool> AnyAsync(int id);

    /// <summary>
    /// Adds the specified entity of type TEntity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add to the repository.</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Updates the specified entity of type TEntity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update in the repository.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Removes the specified entity of type TEntity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove from the repository.</param>
    void Remove(TEntity entity);

    /// <summary>
    /// Changes the state of the specified entity of type TEntity in the repository.
    /// </summary>
    /// <param name="entity">The entity whose state is to be changed.</param>
    /// <param name="state">The new state for the entity.</param>
    void ChangeState(TEntity entity, EntityState state);
}
