using LMS.api.Model;

namespace RoboUnicornsLMS.Services
{
    public class ModuleRequestService : GenericRequestService<Module>, IModuleRequestService
    {
        public ModuleRequestService(HttpClient httpClient, IConfiguration configuration, string serviceName)
            : base(httpClient, configuration, serviceName)
        { }

        public async Task<IEnumerable<Module>> GetModulesByCourseIdAsync(int courseId, CancellationToken cancellation = default)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Module>>($"{_endpointPath}/Course/{courseId}", cancellation) ??
                Enumerable.Empty<Module>();
        }
    }
}
