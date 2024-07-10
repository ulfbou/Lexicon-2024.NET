using Azure.Data.Tables;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TenantManagement.DTOs;
using TenantManagement.Entities;

namespace TenantManagement.Services
{
    public interface IService<TEntity> where TEntity : class, ITableEntity, IIdentifyable
    {
        Task<TEntity?> CreateAsync<TDto>(TDto dto, CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> CreateBulkAsync<TCreateDto>(IEnumerable<TCreateDto> dtos, CancellationToken cancellation = default);
        Task DeleteAsync(string id, CancellationToken cancellation = default);
        Task DeleteBulkAsync(IEnumerable<string> ids, CancellationToken cancellation = default);
        Task<TReadDto?> GetAsync<TReadDto>(string id, CancellationToken cancellation = default);
        Task<IEnumerable<TReadDto>> GetBulkAsync<TReadDto>(Expression<Func<TEntity, bool>>? value, CancellationToken cancellation = default);
        Task<TUpdateDto> UpdateAsync<TUpdateDto>(TUpdateDto dto, CancellationToken cancellation = default) where TUpdateDto : IIdentifyable;
        Task<IEnumerable<TUpdateDto>> UpdateBulkAsync<TUpdateDto>(IEnumerable<TUpdateDto> dtos, CancellationToken cancellation = default) where TUpdateDto : IIdentifyable;
    }
}