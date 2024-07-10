using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TenantManagement.Entities;
using TenantManagement.Repository;
using static Grpc.Core.Metadata;

namespace TenantManagement.Services
{
    public class TenantService<TEntity>(
            ITenantRepository<TEntity> repository,
            ILogger<TenantService<TEntity>> logger,
            IMapper mapper)
        : ITenantService<TEntity> where TEntity : TenantEntity, ITenantEntity, IIdentifyable
    {
        private readonly ITenantRepository<TEntity> _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<TenantService<TEntity>> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<TEntity?> CreateAsync<TDto>(string tenantKey, TDto dto, CancellationToken cancellation = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                return await _repository.CreateAsync(tenantKey ?? Tenant.DefaultTenantKey, entity, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entity");
                throw;
            }
        }

        // Create entity
        public async Task<IEnumerable<TEntity>> CreateBulkAsync<TCreateDto>(string tenantKey, IEnumerable<TCreateDto> dtos, CancellationToken cancellation = default)
        {
            if (dtos == null) throw new ArgumentNullException(nameof(dtos));

            try
            {
                // TODO: Verify that the PartitionKey and RowKey are set correctly. 
                var entities = dtos.Select(_mapper.Map<TCreateDto, TEntity>).ToList();
                return await _repository.CreateBulkAsync(tenantKey ?? Tenant.DefaultTenantKey, entities, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entities");
                throw;
            }
        }

        // Read entity by id
        public async Task<TReadDto?> GetAsync<TReadDto>(string tenantKey, string entityKey, CancellationToken cancellation = default)
        {
            if (entityKey == null) throw new ArgumentNullException(nameof(entityKey));

            try
            {
                TEntity? entity = await _repository.GetAsync(tenantKey ?? Tenant.DefaultTenantKey, entityKey, cancellation);
                if (entity == null) return default;
                return _mapper.Map<TEntity, TReadDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entity");
                throw;
            }
        }

        // Read all entities
        public async Task<IEnumerable<TReadDto>> GetBulkAsync<TReadDto>(string tenantKey, Expression<Func<TEntity, bool>>? filter, CancellationToken cancellation = default)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            try
            {
                IEnumerable<TEntity> entities = await _repository.GetBulkAsync(tenantKey ?? Tenant.DefaultTenantKey, filter, cancellation);
                return entities.Select(_mapper.Map<TEntity, TReadDto>).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entities");
                throw;
            }
        }

        // Update entity
        public async Task<TUpdateDto> UpdateAsync<TUpdateDto>(string tenantKey, TUpdateDto dto, CancellationToken cancellation = default)
            where TUpdateDto : IIdentifyable
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                // TODO: Validate that the dto have valid PartitionKey and RowKey.
                var entity = _mapper.Map<TEntity>(dto);
                TEntity? updatedEntity = await _repository.UpdateAsync(tenantKey ?? Tenant.DefaultTenantKey, entity, cancellation);
                return _mapper.Map<TEntity, TUpdateDto>(updatedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity");
                throw;
            }
        }

        // Update entities
        public async Task<IEnumerable<TUpdateDto>> UpdateBulkAsync<TUpdateDto>(string tenantKey, IEnumerable<TUpdateDto> dtos, CancellationToken cancellation = default)
            where TUpdateDto : IIdentifyable
        {
            if (dtos == null) throw new ArgumentNullException(nameof(dtos));

            try
            {
                // TODO: Validate that the dto have valid PartitionKey and RowKey.
                var entities = dtos.Select(_mapper.Map<TUpdateDto, TEntity>).ToList();
                IEnumerable<TEntity> updatedEntities = await _repository.UpdateBulkAsync(tenantKey ?? Tenant.DefaultTenantKey, entities, cancellation);
                return updatedEntities.Select(_mapper.Map<TEntity, TUpdateDto>).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entities");
                throw;
            }
        }

        // Delete entity
        public async Task DeleteAsync(string tenantKey, string entityKey, CancellationToken cancellation = default)
        {
            if (entityKey == null) throw new ArgumentNullException(nameof(entityKey));

            try
            {
                await _repository.DeleteAsync(tenantKey ?? Tenant.DefaultTenantKey, entityKey, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity");
                throw;
            }
        }

        // Delete entities
        public async Task DeleteBulkAsync(string tenantKey, IEnumerable<string> entityKeys, CancellationToken cancellation = default)
        {
            if (entityKeys == null) throw new ArgumentNullException(nameof(entityKeys));

            try
            {
                await _repository.DeleteBulkAsync(tenantKey ?? Tenant.DefaultTenantKey, entityKeys, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entities");
                throw;
            }
        }
    }
}