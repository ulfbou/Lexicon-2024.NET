using MessengerAPI.Authentication.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MessengerAPI.Authentication.Repositories
{
    /// <summary>
    /// Represents a generic interface for a repository that provides basic CRUD operations for entities of type TEntity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that the repository manages.</typeparam>
    public interface IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Asynchronously retrieves all entities of type TEntity from the repository.
        /// </summary>
        /// <param name="searchQuery">Optional. A string that represents the search query to filter the results.</param>
        /// <param name="pageIndex">Optional. The index of the page to retrieve.</param>
        /// <param name="pageSize">Optional. The size of the page to retrieve.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity.</returns>
        public Task<IEnumerable<TEntity>?> GetAllAsync(
            string? searchQuery = null,
            int pageIndex = 0,
            int pageSize = 10,
            CancellationToken cancellation = default);

        /// <summary>
        /// Asynchronously retrieves all entities of type TEntity that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">A delegate to test each element for a condition.</param>
        /// <param name="searchQuery">Optional. A string that represents the search query to filter the results.</param>
        /// <param name="pageIndex">Optional. The index of the page to retrieve.</param>
        /// <param name="pageSize">Optional. The size of the page to retrieve.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity that satisfy the predicate.</returns>
        public Task<IEnumerable<TEntity>?> FindAsync(
            Expression<Func<TEntity, bool>>? predicate,
            int pageIndex = 0,
            int pageSize = 10,
            CancellationToken cancellation = default);

        /// <summary>
        /// Asynchronously retrieves an entity of type TEntity with the specified id in the repository.
        /// </summary>
        /// <param name="id">The id of the entity to locate in the repository.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity of type TEntity with the specified id, or null if it does not exist.</returns>
        public Task<TEntity?> GetAsync(int id, CancellationToken cancellation = default);

        /// <summary>
        /// Asynchronously determines whether an entity of type TEntity with the specified id exists in the repository.
        /// </summary>
        /// <param name="id">The id of the entity to locate in the repository.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if an entity with the specified id exists, false otherwise.</returns>
        public Task<bool> AnyAsync(int id, CancellationToken cancellation = default);


        /// <summary>
        /// Asynchronously determines whether an entity of type TEntity with the specified id exists in the repository.
        /// </summary>
        /// <param name="predicate">A delegate to test each element for a condition.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if an entity with the specified id exists, false otherwise.</returns>
        public Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>>? predicate,
            CancellationToken cancellation = default);

        /// <summary>
        /// Adds the specified entity of type TEntity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add to the repository.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <exception cref="OperationCanceledException">Thrown when the operation was canceled.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the operation failed.</exception>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public void Add(TEntity entity);

        /// <summary>
        /// Updates the specified entity of type TEntity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update in the repository.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public void Update(TEntity entity);

        /// <summary>
        /// Removes the specified entity of type TEntity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove from the repository.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public void Remove(TEntity entity);

        /// <summary>
        /// Deletes the specified entity of type TEntity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove from the repository.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public void Delete(TEntity entity);

        /// <summary>
        /// Changes the state of the specified entity of type TEntity in the repository.
        /// </summary>
        /// <param name="entity">The entity whose state is to be changed.</param>
        /// <param name="state">The new state for the entity.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public void ChangeState(TEntity entity, EntityState state);

        /// <summary>
        /// Asynchronously gets the number of entities of type TEntity in the repository.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities of type TEntity in the repository.</returns>
        public Task<int> CountAsync(CancellationToken cancellation = default);
    }
}
