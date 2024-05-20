using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Extensions;

public static class WebAppExtensions
{
    public static async Task SeedDataAsync(this IApplicationBuilder app) 
    { 
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<TournamentAPIContext>();

            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();

            try
            {
                await SeedData.InitAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
