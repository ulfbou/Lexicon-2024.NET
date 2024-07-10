using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using TenantManagement.Extensions;

namespace TenantManagement.Repository
{
    public class AzuriteTableRepository<TEntity> : IRepository<TEntity> where TEntity : class, ITableEntity
    {
        private readonly IConfiguration _configuration;
        private readonly string _entityName;
        private readonly TableClient _tableClient;

        public AzuriteTableRepository(IConfiguration configuration)
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

        public async Task<TEntity?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
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

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellation = default)
        {
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

        public async Task<IEnumerable<TEntity>> CreateBulkAsync(IEnumerable<TEntity> entity, CancellationToken cancellation = default)
        {
            try
            {
                var tasks = entity.Select(e => CreateAsync(e, cancellation));
                var result = await Task.WhenAll(tasks);
                return result.Where(e => e != null).Select(e => e!);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetBulkAsync(Expression<Func<TEntity, bool>>? filter, CancellationToken cancellation = default)
        {
            try
            {
                AsyncPageable<TEntity> query = _tableClient.QueryAsync(filter: filter, cancellationToken: cancellation);
                return await query.ToAsyncList(cancellation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> UpdateBulkAsync(List<TEntity> entities, CancellationToken cancellation = default)
        {
            try
            {
                var tasks = entities.Select(e => UpdateAsync(e, cancellation));
                return await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task DeleteBulkAsync(IEnumerable<string> ids, CancellationToken cancellation = default)
        {
            try
            {
                var tasks = ids.Select(id => DeleteAsync(id, id, cancellation));
                return Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}