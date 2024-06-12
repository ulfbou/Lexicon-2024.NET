using MessengerAPI.Authentication.Data;
using MessengerAPI.Authentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace MessengerAPI.Authentication.Repositories
{
    // Generate professional level documentation and code for a base class for all repositories that implements IRepository<TEntity>

    /// <summary>
    /// Represents a base class for all repositories that implements IRepository<TEntity>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity in the repository.</typeparam>
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        protected static readonly ILogger<TEntity> _logger = new Logger<TEntity>(new LoggerFactory());
        protected static readonly AsyncRetryPolicy RetryPolicy = Policy
            .Handle<DbUpdateException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(1, attempt)), // Exponential backoff
                onRetry: (exception, timeSpan, context) =>
                {
                    // Log the exception and the retry attempt
                    _logger.LogWarning(exception, $"Retry attempt due to exception: {exception.Message}");
                });

        protected const int pageSizeMax = 100;

        protected readonly DbSet<TEntity> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity}"/> class.
        /// </summary>
        public RepositoryBase(DbSet<TEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Asynchronously retrieves all entities of type TEntity from the repository.
        /// </summary>
        /// <param name="searchQuery">Optional. A string that represents the search query to filter the results.</param>
        /// <param name="pageIndex">Optional. The index of the page to retrieve.</param>
        /// <param name="pageSize">Optional. The size of the page to retrieve.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity.</returns>
        public virtual async Task<IEnumerable<TEntity>?> GetAllAsync(
            string? searchQuery = null,
            int pageIndex = 0,
            int pageSize = 10,
            CancellationToken cancellation = default)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            if (pageSize > pageSizeMax) pageSize = pageSizeMax;

            IQueryable<TEntity> query = _repository;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(e => e.SearchableContent.Contains(searchQuery));
            }

            return await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves all entities of type TEntity that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">A delegate to test each element for a condition.</param>
        /// <param name="searchQuery">Optional. A string that represents the search query to filter the results.</param>
        /// <param name="pageIndex">Optional. The index of the page to retrieve.</param>
        /// <param name="pageSize">Optional. The size of the page to retrieve.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TEntity that satisfy the predicate.</returns>
        public virtual async Task<IEnumerable<TEntity>?> FindAsync(
            Expression<Func<TEntity, bool>>? predicate,
            int pageIndex = 0,
            int pageSize = 10,
            CancellationToken cancellation = default)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            if (pageSize > pageSizeMax) pageSize = pageSizeMax;

            IQueryable<TEntity> query = _repository;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves an entity of type TEntity with the specified id in the repository.
        /// </summary>
        /// <param name="id">The id of the entity to locate in the repository.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity of type TEntity with the specified id, or null if it does not exist.</returns>
        public virtual async Task<TEntity?> GetAsync(int id, CancellationToken cancellation = default)
        {
            return (await FindAsync(entity => entity.Id == id, 1, 1, cancellation))?.FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously determines whether an entity of type TEntity with the specified id exists in the repository.
        /// </summary>
        /// <param name="id">The id of the entity to locate in the repository.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if an entity with the specified id exists, false otherwise.</returns>
        public virtual async Task<bool> AnyAsync(int id, CancellationToken cancellation = default)
        {
            return await _repository.AnyAsync(e => e.Id == id, cancellation);
        }

        /// <summary>
        /// Asynchronously determines whether an entity of type TEntity with the specified id exists in the repository.
        /// </summary>
        /// <param name="predicate">A delegate to test each element for a condition.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if an entity with the specified id exists, false otherwise.</returns>
        public virtual async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>>? predicate,
            CancellationToken cancellation = default)
        {
            return await _repository.FindAsync(predicate, cancellation) != null;
        }

        /// <summary>
        /// Adds the specified entity of type TEntity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add to the repository.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <exception cref="OperationCanceledException">Thrown when the operation was canceled.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the operation failed.</exception>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellation = default)
        {
            return await RetryPolicy.ExecuteAsync(async () =>
            {
                await _repository.AddAsync(entity, cancellation);
                return entity;
            });
        }

        /// <summary>
        /// Adds the specified entity of type TEntity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add to the repository.</param>
        /// <param name="cancellation">Optional. A cancellation token that can be used to cancel the operation.</param>
        /// <exception cref="OperationCanceledException">Thrown when the operation was canceled.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the operation failed.</exception>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public virtual void Add(TEntity entity)
        {
            try
            {
                _repository.Add(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while adding an entity of type {typeof(TEntity).Name} to the repository.", ex);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the specified entity of type TEntity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update in the repository.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public virtual void Update(TEntity entity)
        {
            try
            {
                _repository.Update(entity);
            }
            catch (OperationCanceledException ex)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("An error occurred while updating the entity in the repository.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the entity in the repository.", ex);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("An error occurred while updating the entity in the repository.", ex);
            }
            catch (DbException ex)
            {
                throw new Exception("An error occurred while updating the entity in the repository.", ex);
            }
        }

        /// <summary>
        /// Removes the specified entity of type TEntity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove from the repository.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public virtual void Remove(TEntity entity)
        {
            EntityEntry<TEntity> entry = null!;
            try
            {
                entry = _repository.Remove(entity);
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                {
                    throw new Exception("An error occurred while adding the entity to the repository.", ex);
                }
            }

            if (entry is null || entry.State != EntityState.Detached)
            {
                throw new InvalidOperationException("The entity could not be updated in the repository.");
            }
        }

        /// <summary>
        /// Deletes the specified entity of type TEntity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove from the repository.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public virtual void Delete(TEntity entity)
        {
            ChangeState(entity, EntityState.Deleted);
        }

        /// <summary>
        /// Changes the state of the specified entity of type TEntity in the repository.
        /// </summary>
        /// <param name="entity">The entity whose state is to be changed.</param>
        /// <param name="state">The new state for the entity.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>The operation is not persisted before calling SaveChanges or SaveChangesAsync in the DbContext.</remarks>
        public virtual void ChangeState(TEntity entity, EntityState state)
        {
            _repository.Entry(entity).State = state;
        }

        /// <summary>
        /// Asynchronously gets the number of entities of type TEntity in the repository.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities of type TEntity in the repository.</returns>
        public virtual async Task<int> CountAsync(CancellationToken cancellation = default)
        {
            return await _repository.CountAsync(cancellation);
        }
    }
}
