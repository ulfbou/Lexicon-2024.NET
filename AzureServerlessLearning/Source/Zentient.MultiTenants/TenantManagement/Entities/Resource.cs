using Azure.Data.Tables;

namespace TenantManagement.Entities
{
    public class Resource : TenantEntity, ITableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}