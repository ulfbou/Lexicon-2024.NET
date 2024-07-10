using LMS.api.Configurations;
using NuGet.Configuration;
using RoboUnicornsLMS.Services;

namespace LMS.api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestService<TService, TImplementation>(this IServiceCollection services, IConfiguration configuration)
            where TService : class
            where TImplementation : class, TService
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            string serviceName = typeof(TService).Name;
            IConfigurationSection requestServiceConfig = configuration.GetSection($"RequestServices:{serviceName}");

            var baseAddress = requestServiceConfig.GetValue<string>("BaseAddress") ?? throw new InvalidOperationException($"Base address not found for service '{serviceName}'.");

            services.AddHttpClient<TService, TImplementation>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                var defaultRequestHeaders = requestServiceConfig.GetSection("DefaultRequestHeaders").Get<Dictionary<string, string>>()
                    ?? throw new InvalidOperationException($"Default request headers not found for service '{serviceName}'.");

                foreach (var header in defaultRequestHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }).Services.AddScoped<TService, TImplementation>(serviceProvider =>
            {
                var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(TService).Name);
                return Activator.CreateInstance(typeof(TImplementation), httpClient, configuration, serviceName) as TImplementation ?? throw new InvalidOperationException($"Could not create instance of {typeof(TImplementation).Name}.");
            });

            return services;
        }
    }
}
