using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;

public interface IRepository<TEntity> where TEntity : IEntity
{
    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves an entity of type TEntity with the specified id from the repository.
    /// </summary>
    /// <param name="id">The id of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the TEntity object if found, null otherwise.</returns>
    Task<TEntity?> GetAsync(int id);

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity that satisfy the predicate.</returns>
    Task<IEnumerable<TEntity>?> FindAsync(Func<TEntity, bool> predicate);

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
