namespace TenantManagement.Entities
{
    public class TenantEntity : TableEntity, ITenantEntity
    {
        public string TenantId => PartitionKey;
    }
}