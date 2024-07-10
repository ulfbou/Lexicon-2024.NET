using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TenantManagement;
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
        services.AddScoped(typeof(IRepository<>), typeof(TenantTableRepository<>));
        services.AddScoped(typeof(ITenantService<>), typeof(TenantService<>));

        // Register AutoMapper
        services.AddAutoMapper(Assembly.GetAssembly(typeof(CourseMappingProfile)));

        // Register Tenant Identification Middleware
        services.AddSingleton<TenantIdMiddleware>();
    })
    .Build();

host.Run();