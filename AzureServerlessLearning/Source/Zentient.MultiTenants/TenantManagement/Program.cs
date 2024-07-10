using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
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
        services.AddScoped(typeof(IRepository<>), typeof(AzuriteTableRepository<>));
        services.AddScoped(typeof(IService<>), typeof(TenantService<>));

        // Register AutoMapper
        services.AddAutoMapper(Assembly.GetAssembly(typeof(CourseMappingProfile)));
    })
    .Build();

host.Run();