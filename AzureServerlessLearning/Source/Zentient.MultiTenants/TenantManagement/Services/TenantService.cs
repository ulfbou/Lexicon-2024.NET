using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TenantManagement.Entities;
using TenantManagement.Repository;

namespace TenantManagement.Services
{
    public class TenantService<TEntity>(
            IRepository<TEntity> repository,
            ILogger<TenantService<TEntity>> logger,
            IMapper mapper)
        where TEntity : class, ITenantEntity, IIdentifyable
    {
        private readonly IRepository<TEntity> _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<TenantService<TEntity>> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<TEntity?> CreateAsync<TDto>(TDto dto, CancellationToken cancellation = default)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                return await _repository.CreateAsync(entity, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entity");
                throw;
            }
        }

        // Create entity
        public async Task<IEnumerable<TEntity>> CreateBulkAsync<TCreateDto>(IEnumerable<TCreateDto> dtos, CancellationToken cancellation = default)
        {
            try
            {
                var entities = dtos.Select(_mapper.Map<TCreateDto, TEntity>).ToList();
                return await _repository.CreateBulkAsync(entities, cancellation);
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
            try
            {
                TEntity? entity = await _repository.GetAsync(tenantKey, entityKey, cancellation);
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
        public async Task<IEnumerable<TReadDto>> GetBulkAsync<TReadDto>(Expression<Func<TEntity, bool>>? filter, CancellationToken cancellation = default)
        {
            try
            {
                IEnumerable<TEntity> entities = await _repository.GetBulkAsync(filter, cancellation);
                return entities.Select(_mapper.Map<TEntity, TReadDto>).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entities");
                throw;
            }
        }

        // Update entity
        public async Task<TUpdateDto> UpdateAsync<TUpdateDto>(TUpdateDto dto, CancellationToken cancellation = default)
            where TUpdateDto : IIdentifyable
        {
            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                TEntity? updatedEntity = await _repository.UpdateAsync(entity, cancellation);
                return _mapper.Map<TEntity, TUpdateDto>(updatedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity");
                throw;
            }
        }

        // Update entities
        public async Task<IEnumerable<TUpdateDto>> UpdateBulkAsync<TUpdateDto>(IEnumerable<TUpdateDto> dtos, CancellationToken cancellation = default)
            where TUpdateDto : IIdentifyable
        {
            try
            {
                var entities = dtos.Select(_mapper.Map<TUpdateDto, TEntity>).ToList();
                IEnumerable<TEntity> updatedEntities = await _repository.UpdateBulkAsync(entities, cancellation);
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
            try
            {
                await _repository.DeleteAsync(tenantKey, entityKey, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity");
                throw;
            }
        }

        // Delete entities
        public async Task DeleteBulkAsync(IEnumerable<string> ids, CancellationToken cancellation = default)
        {
            try
            {
                await _repository.DeleteBulkAsync(ids, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entities");
                throw;
            }
        }
    }
}