using MachineryFleet.Core.Repository;
using MachineryFleet.Core.Services;
using MachineryFleet.Persistence.Data;
using MachineryFleet.Persistence.Repository;
using MachineryFleet.Web.Components;
using Microsoft.EntityFrameworkCore;
using Zentient.Repository;

namespace MachineryFleet.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddRazorPages();
            builder.WebHost.UseStaticWebAssets();

            builder.Services.AddDbContext<DbContext, MachineryFleetDbContext>(options =>
                options.UseInMemoryDatabase("MachineryFleetDb"));
            //options.UseSqlServer(
            //        builder.Configuration.GetConnectionString("DefaultConnection"),
            //        b => b.MigrationsAssembly("MachineryFleet.Web")));

            // Dependency Injection
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMachineRepository, MachineRepository>();
            builder.Services.AddScoped<IMachineService, MachineService>();

            var app = builder.Build();

            await SeedData.Initialize(app);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
