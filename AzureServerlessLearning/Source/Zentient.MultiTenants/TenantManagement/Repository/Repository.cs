
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TenantManagement.Entities;

namespace TenantManagement.Repository
{
    public class Repository<TContext, TEntity>(
        TContext context,
        ILogger<Repository<TContext, TEntity>> logger,
        IMapper mapper)
        : IRepository<TContext, TEntity>
        where TContext : DbContext
        where TEntity : class, ITenantEntity, IIdentifyable
    {
        private readonly TContext _context = context
            ?? throw new ArgumentNullException(nameof(context));
        private readonly DbSet<TEntity> _entities = context.Set<TEntity>()
            ?? throw new ArgumentNullException(nameof(context));
        private readonly ILogger<Repository<TContext, TEntity>> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

        // Create entity
        public async Task<TEntity?> CreateAsync(TEntity entity, CancellationToken cancellation = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                var result = await _entities.AddAsync(entity, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entity");
                throw;
            }
        }

        // Create entities
        public async Task<IEnumerable<TEntity>> CreateBulkAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default)
        {
            if (entities == null) throw new ArgumentNullException("entities");

            try
            {
                await _entities.AddRangeAsync(entities, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entities");
                throw;
            }
        }

        // Read entity by id
        public async Task<TEntity?> GetAsync(string id, CancellationToken cancellation = default)
        {
            if (id == null) throw new ArgumentNullException("id");

            try
            {
                return await _entities.FindAsync(new object[] { id }, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entity");
                throw;
            }
        }

        // Read many entities
        public async Task<IEnumerable<TEntity>> GetBulkAsync(Expression<Func<TEntity, bool>>? value, CancellationToken cancellation = default)
        {
            try
            {
                return await (value == null ? _entities : _entities.Where(value)).ToListAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entities");
                throw;
            }
        }

        // Update entity
        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellation = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                EntityEntry<TEntity> result = _entities.Update(entity);
                await _context.SaveChangesAsync(cancellation);
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity");
                throw;
            }
        }

        // Update many entities
        public async Task<IEnumerable<TEntity>> UpdateBulkAsync(List<TEntity> entities, CancellationToken cancellation = default)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                _entities.UpdateRange(entities);
                await _context.SaveChangesAsync(cancellation);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entities");
                throw;
            }
        }

        // Delete entity
        public async Task DeleteAsync(string id, CancellationToken cancellation = default)
        {
            if (id == null) throw new ArgumentNullException("id");
            try
            {
                TEntity? entity = await GetAsync(id, cancellation);
                if (entity == null)
                {
                    return;
                }

                _entities.Remove(entity);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity");
                throw;
            }
        }

        // Delete many entities
        public async Task DeleteBulkAsync(IEnumerable<string> ids, CancellationToken cancellation = default)
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));

            try
            {
                IEnumerable<TEntity> entities = await _entities.Where(entity => ids.Contains(entity.Id)).ToListAsync(cancellation);
                if (entities.Any())
                {
                    return;
                }

                _entities.RemoveRange(entities);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entities");
                throw;
            }
        }
    }
}