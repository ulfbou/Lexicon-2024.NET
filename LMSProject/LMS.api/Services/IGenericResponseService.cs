using LMS.api.Model;

namespace LMS.api.Services
{
    public interface IGenericResponseService<TEntity> : IGenericResponseService<TEntity, int>
        where TEntity : class, IEntity<int>
    { }

    public interface IGenericResponseService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : notnull
    {
        Task<List<TEntity>> GetAsync(CancellationToken cancellation = default);
        Task<TEntity?> GetAsync(TKey id, CancellationToken cancellation = default);
        Task AddAsync(TEntity entity, CancellationToken cancellation = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellation = default);
        Task DeleteAsync(TKey id, CancellationToken cancellation = default);
    }
}
