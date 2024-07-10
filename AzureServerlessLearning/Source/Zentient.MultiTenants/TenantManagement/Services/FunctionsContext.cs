using Microsoft.EntityFrameworkCore;

namespace TenantManagement.Services
{
    public class FunctionsContext : DbContext
    {
        public FunctionsContext(DbContextOptions<FunctionsContext> options)
            : base(options)
        {
        }
    }
}