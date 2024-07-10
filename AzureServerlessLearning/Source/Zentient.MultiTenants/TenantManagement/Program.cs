using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TenantManagement;
using TenantManagement.Entities;
using TenantManagement.Middleware;
using TenantManagement.Profiles;
using TenantManagement.Repository;
using TenantManagement.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        // Set up the logger factory
        services.AddLogging();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Register services: IRepository<> and IService<>
        services.AddScoped(typeof(ITenantRepository<>), typeof(TenantTableRepository<>));
        services.AddScoped(typeof(ITenantService<>), typeof(TenantService<>));
        services.AddScoped<ITenantRepository<Course>, TenantTableRepository<Course>>();
        services.AddScoped<ITenantService<Course>, TenantService<Course>>();

        // Register AutoMapper
        services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

        // Register Tenant Identification Middleware
        services.AddSingleton<TenantIdMiddleware>();
    })
    .Build();

host.Run();