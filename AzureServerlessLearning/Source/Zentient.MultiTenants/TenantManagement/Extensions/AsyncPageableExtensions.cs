using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TenantManagement.Extensions
{
    public static class AsyncPageableExtensions
    {
        public static async Task<IEnumerable<T>> ToAsyncList<T>(this AsyncPageable<T> pageable, CancellationToken cancellation = default) where T : notnull
        {
            var entities = new List<T>();
            await foreach (var page in pageable.AsPages())
            {
                entities.AddRange(page.Values);
            }
            return entities;
        }
    }
}