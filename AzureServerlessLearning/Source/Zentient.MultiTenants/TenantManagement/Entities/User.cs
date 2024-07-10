using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TenantManagement.Entities
{
    public class User : TenantEntity, ITenantEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public User Parent { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Role> Roles { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}