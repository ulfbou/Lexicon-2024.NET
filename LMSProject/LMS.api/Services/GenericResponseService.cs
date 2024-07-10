using LMS.api.Data;
using LMS.api.Model;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.api.Services
{
    // Generate professional documentation

    /// <summary>
    /// Generic response service for entity with integer key type.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <seealso cref="IGenericResponseService{TEntity}" />
    /// <seealso cref="IEntity{int}" />
    /// <return cref="int">Key type.</return>
    public class GenericResponseService<TEntity> : GenericResponseService<TEntity, int>, IGenericResponseService<TEntity>
        where TEntity : class, IEntity<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResponseService{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// 
        public GenericResponseService(ApplicationDbContext context) : base(context)
        {
        }
    }

    public class GenericResponseService<TEntity, TKey> : IGenericResponseService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : notnull
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericResponseService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAsync(CancellationToken cancellation = default)
        {
            return await _dbSet.ToListAsync(cancellation);
        }

        public async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellation = default)
        {
            return await _dbSet.FindAsync(id, cancellation);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellation = default)
        {
            await _dbSet.AddAsync(entity, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellation = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(TKey id, CancellationToken cancellation = default)
        {
            var entity = await _dbSet.FindAsync(id, cancellation);
            if (entity == null)
            {
                // TODO: Handle not found
                return;
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
