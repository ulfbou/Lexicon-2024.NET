using Azure.Data.Tables;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TenantManagement.Entities;
using TenantManagement.Services;

namespace TenantManagement.Repository
{
    public interface ITenantRepository<TEntity> : ITenantEntityRepository<TEntity>, ITenantBulkRepository<TEntity>
        where TEntity : TenantEntity, ITenantEntity
    { }

    public interface ITenantEntityRepository<TEntity> where TEntity : TenantEntity, ITenantEntity
    {
        Task<TEntity?> CreateAsync(string tenantKey, TEntity entity, CancellationToken cancellation = default);
        Task<TEntity?> GetAsync(string tenantKey, string entityKey, CancellationToken cancellation = default);
        Task<TEntity> UpdateAsync(string tenantKey, TEntity entity, CancellationToken cancellation = default);
        Task DeleteAsync(string tenantKey, string entityKey, CancellationToken cancellation = default);
    }

    public interface ITenantBulkRepository<TEntity> where TEntity : TenantEntity, ITenantEntity
    {
        Task<IEnumerable<TEntity>> CreateBulkAsync(string tenantKey, IEnumerable<TEntity> entity, CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> GetBulkAsync(string tenantKey, Expression<Func<TEntity, bool>>? value, CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> UpdateBulkAsync(string tenantKey, IEnumerable<TEntity> entities, CancellationToken cancellation = default);
        Task DeleteBulkAsync(string tenantKey, IEnumerable<string> ids, CancellationToken cancellation = default);
    }

    public interface IRepository<TEntity> : IEntityRepository<TEntity>, IBulkRepository<TEntity>
        where TEntity : class, ITableEntity
    { }

    public interface IEntityRepository<TEntity> where TEntity : class, ITableEntity
    {
        Task<TEntity?> CreateAsync(string tenantKey, TEntity entity, CancellationToken cancellation = default);
        Task<TEntity?> GetAsync(string tenantKey, string entityKey, CancellationToken cancellation = default);
        Task<TEntity> UpdateAsync(string tenantKey, TEntity entity, CancellationToken cancellation = default);
        Task DeleteAsync(string tenantKey, string entityKey, CancellationToken cancellation = default);
    }

    public interface IBulkRepository<TEntity> where TEntity : class, ITableEntity
    {
        Task<IEnumerable<TEntity>> CreateBulkAsync(IEnumerable<TEntity> entity, CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> GetBulkAsync(Expression<Func<TEntity, bool>>? value, CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> UpdateBulkAsync(List<TEntity> entities, CancellationToken cancellation = default);
        Task DeleteBulkAsync(IEnumerable<string> ids, CancellationToken cancellation = default);
    }

    public interface IRepository<TContext, TEntity> : IEntityRepository<TContext, TEntity>, IBulkRepository<TContext, TEntity>
        where TContext : DbContext
        where TEntity : class, ITableEntity, IIdentifyable
    { }

    public interface IEntityRepository<TContext, TEntity>
        where TContext : DbContext
        where TEntity : class, ITableEntity, IIdentifyable
    { }

    public interface IBulkRepository<TContext, TEntity>
        where TContext : DbContext
        where TEntity : class, ITableEntity, IIdentifyable
    { }
}