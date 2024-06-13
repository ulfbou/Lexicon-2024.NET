
using MachineryFleet.Core.Repository;
using MachineryFleet.Core.Services;
using MachineryFleet.Persistence.Data;
using MachineryFleet.Persistence.Repository;
using MachineryFleet.Web;
using MachineryFleet.Web.Components;
using Microsoft.EntityFrameworkCore;
using Zentient.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.WebHost.UseStaticWebAssets();


builder.Services.AddDbContext<DbContext, MachineryFleetDbContext>(options =>
    options.UseInMemoryDatabase("MachineryFleetDb"));
// This commented code should be kept for future reference
//options.UseSqlServer(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        b => b.MigrationsAssembly("MachineryFleet.Web")));

builder.Services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMachineService, MachineService>();

var app = builder.Build();

await SeedData.Initialize(app);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();