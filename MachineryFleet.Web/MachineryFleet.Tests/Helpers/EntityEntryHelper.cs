using MachineryFleet.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace MachineryFleet.Tests.Helpers
{
    public static class EntityEntryHelper
    {
        public static EntityEntry<T> CreateEntityEntry<T>(T entity, EntityState state = EntityState.Added) where T : class
        {
            // Creating a DbContextOptions to create a temporary DbContext
            var options = new DbContextOptionsBuilder<MachineryFleetDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            // Creating a temporary context to generate a real EntityEntry<T>
            using (var context = new MachineryFleetDbContext(options))
            {
                var entityEntry = context.Entry(entity);
                entityEntry.State = state;
                return entityEntry;
            }
        }
    }
}
