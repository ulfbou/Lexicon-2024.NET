using Azure.Data.Tables;

namespace TenantManagement.Entities
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class Tenant : TenantEntity, ITableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}