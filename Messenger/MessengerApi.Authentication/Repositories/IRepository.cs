using MessengerAPI.Authentication.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MessengerAPI.Authentication.Repositories
{
    /// <summary>
    /// Represents a generic interface for a repository that provides basic CRUD operations for entities of type TEntity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that the repository manages.</typeparam>
    public interface IRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Asynchronously retrieves all entities of type TEntity from the repository with the specified include properties.
        /// </summary>
        /// <param name="includeProperties">An array of expressions that represent the properties to include in the result.</param>
        /// <param name="pageIndex">Optional. The index of the page to retrieve.</param>
        /// <param name="pageSize">Optional. The size of the page to retrieve.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity with the specified include properties.</returns>
        public Task<IEnumerable<TEntity>?> GetAllAsync(
            Expression<Func<TEntity, object>>[]? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10,
            CancellationToken cancellation = default);
    }
}
