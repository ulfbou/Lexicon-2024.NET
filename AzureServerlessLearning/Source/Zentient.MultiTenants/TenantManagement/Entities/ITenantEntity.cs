using Azure.Data.Tables;

namespace TenantManagement.Entities
{
    public interface ITenantEntity : ITableEntity
    {
        string TenantId { get; }
    }
}