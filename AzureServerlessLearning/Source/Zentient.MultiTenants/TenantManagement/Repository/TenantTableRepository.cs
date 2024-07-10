using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using TenantManagement.Entities;
using TenantManagement.Extensions;

namespace TenantManagement.Repository
{
    public class TenantTableRepository<TEntity> : ITenantRepository<TEntity> where TEntity : TenantEntity, ITenantEntity
    {
        private readonly IConfiguration _configuration;
        private readonly string _entityName;
        private readonly TableClient _tableClient;

        public TenantTableRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _entityName = typeof(TEntity).Name;

            var connectionString = _configuration.GetSection("ConnectionStrings:AzureTableStorage").Get<string>()
                ?? throw new InvalidOperationException("Missing 'ConnectionStrings:AzureTableStorage' in configuration.");
            var tableServiceClient = new TableServiceClient(connectionString)
                ?? throw new InvalidOperationException("Could not create TableServiceClient.");
            _tableClient = tableServiceClient.GetTableClient(_entityName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<TEntity?> CreateAsync(string tenantKey, TEntity entity, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(tenantKey)) throw new ArgumentNullException(nameof(tenantKey));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                // Set the PartitionKey and RowKey.
                entity.PartitionKey = tenantKey;
                entity.RowKey = Guid.NewGuid().ToString();
                await _tableClient.AddEntityAsync(entity, cancellationToken);
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TEntity?> GetAsync(string tenantKey, string entityKey, CancellationToken cancellation = default)
        {
            if (!string.IsNullOrEmpty(tenantKey)) throw new ArgumentNullException(nameof(tenantKey));
            if (entityKey == null) throw new ArgumentNullException(nameof(entityKey));

            try
            {
                var response = await _tableClient.GetEntityAsync<TEntity>(tenantKey, entityKey, null, cancellation);
                return response?.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TEntity> UpdateAsync(string tenantKey, TEntity entity, CancellationToken cancellation = default)
        {
            if (!string.IsNullOrEmpty(tenantKey)) throw new ArgumentNullException(nameof(tenantKey));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Response? response = null;

            try
            {
                response = await _tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace, cancellation);
            }
            catch (Exception)
            {
                throw;
            }

            if (response == null) throw new InvalidOperationException($"Could not update entity type '{_entityName}' with Partition Key '{entity.PartitionKey}' and Row Key '{entity.RowKey}'.");

            return entity;
        }

        public async Task DeleteAsync(string tenantKey, string entityKey, CancellationToken cancellation = default)
        {
            if (!string.IsNullOrEmpty(tenantKey)) throw new ArgumentNullException(nameof(tenantKey));
            if (entityKey == null) throw new ArgumentNullException(nameof(entityKey));

            try
            {
                var response = await _tableClient.GetEntityAsync<TEntity>(tenantKey, entityKey, ["PartitionKey"], cancellation);
                if (response == null)
                {
                    throw new InvalidOperationException($"DELETE ERROR: Missing entity type '{_entityName}' with Partition Key '{tenantKey}' and Row Key '{entityKey}'.");
                }

                var entity = response.Value;
                await _tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey, entity.ETag, cancellation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> CreateBulkAsync(string tenantKey, IEnumerable<TEntity> entity, CancellationToken cancellation = default)
        {
            if (!string.IsNullOrEmpty(tenantKey)) throw new ArgumentNullException(nameof(tenantKey));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                var tasks = entity.Select(e => CreateAsync(e.PartitionKey, e, cancellation));
                var result = await Task.WhenAll(tasks);
                return result.Where(e => e != null).Select(e => e!);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetBulkAsync(string tenantKey, Expression<Func<TEntity, bool>>? filter, CancellationToken cancellation = default)
        {
            if (!string.IsNullOrEmpty(tenantKey)) throw new ArgumentNullException(nameof(tenantKey));
            try
            {
                // TODO: Handle null filter to retrieve all entities.
                AsyncPageable<TEntity> query = _tableClient.QueryAsync(filter: filter, cancellationToken: cancellation);
                return await query.ToAsyncList(cancellation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> UpdateBulkAsync(string tenantKey, IEnumerable<TEntity> entities, CancellationToken cancellation = default)
        {
            if (!string.IsNullOrEmpty(tenantKey)) throw new ArgumentNullException(nameof(tenantKey));
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                var tasks = entities.Select(e => UpdateAsync(tenantKey, e, cancellation));
                return await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task DeleteBulkAsync(string tenantKey, IEnumerable<string> ids, CancellationToken cancellation = default)
        {
            if (!string.IsNullOrEmpty(tenantKey)) throw new ArgumentNullException(nameof(tenantKey));
            if (ids == null) throw new ArgumentNullException(nameof(ids));

            try
            {
                var tasks = ids.Select(id => DeleteAsync(tenantKey, id, cancellation));
                return Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}