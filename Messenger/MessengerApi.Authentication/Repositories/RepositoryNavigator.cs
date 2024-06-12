using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MessengerAPI.Authentication.Models;

namespace MessengerAPI.Authentication.Repositories
{
    public class Repository<TEntity>
        : RepositoryBase<TEntity>, IRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public Repository(DbSet<TEntity> repository) : base(repository)
        {
        }

        // Generate a new summary for GetAllAsync that describes the method's functionality
        /// <summary>
        /// Asynchronously retrieves all entities of type TEntity from the repository with the specified include properties.
        /// </summary>
        /// <param name="includeProperties">An array of expressions that represent the properties to include in the result.</param>
        /// <param name="pageIndex">Optional. The index of the page to retrieve.</param>
        /// <param name="pageSize">Optional. The size of the page to retrieve.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity with the specified include properties.</returns>
        public virtual async Task<IEnumerable<TEntity>?> GetAllAsync(
            Expression<Func<TEntity, object>>[]? includeProperties,
            int pageIndex = 0,
            int pageSize = 10,
            CancellationToken cancellation = default)
        {
            IQueryable<TEntity> query = _repository;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
