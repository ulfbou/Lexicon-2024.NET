using LMS.api.Model;

namespace RoboUnicornsLMS.Services
{
    public class ActivityRequestService : GenericRequestService<Activity>, IActivityRequestService
    {
        public ActivityRequestService(HttpClient httpClient, IConfiguration configuration, string serviceName)
            : base(httpClient, configuration, serviceName)
        { }

        public async Task<IEnumerable<Activity>> GetActivitiesByModuleIdAsync(int moduleId, CancellationToken cancellation = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_endpointPath}/module/{moduleId}", cancellation);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<IEnumerable<Activity>>();
                return result ?? Enumerable.Empty<Activity>();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"An error occurred while attempting to retrieve data from {_endpointPath}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new TaskCanceledException($"The request to {_endpointPath} was cancelled.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while attempting to retrieve data from {_endpointPath}.", ex);
            }
        }
    }
}
