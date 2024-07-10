using Azure.Data.Tables;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TenantManagement.DTOs;
using TenantManagement.Entities;

namespace TenantManagement.Services
{
    public interface ITenantService<TEntity> where TEntity : TenantEntity, ITenantEntity, IIdentifyable
    {
        Task<TEntity?> CreateAsync<TDto>(string tenantKey, TDto dto, CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> CreateBulkAsync<TCreateDto>(string tenantKey, IEnumerable<TCreateDto> dtos, CancellationToken cancellation = default);
        Task<TReadDto?> GetAsync<TReadDto>(string tenantKey, string entityKey, CancellationToken cancellation = default);
        Task<IEnumerable<TReadDto>> GetBulkAsync<TReadDto>(string tenantKey, Expression<Func<TEntity, bool>>? value, CancellationToken cancellation = default);
        Task<TUpdateDto> UpdateAsync<TUpdateDto>(string tenantKey, TUpdateDto dto, CancellationToken cancellation = default) where TUpdateDto : IIdentifyable;
        Task<IEnumerable<TUpdateDto>> UpdateBulkAsync<TUpdateDto>(string tenantKey, IEnumerable<TUpdateDto> dtos, CancellationToken cancellation = default) where TUpdateDto : IIdentifyable;
        Task DeleteAsync(string tenantKey, string entityKey, CancellationToken cancellation = default);
        Task DeleteBulkAsync(string tenantKey, IEnumerable<string> entityKeys, CancellationToken cancellation = default);
    }
}