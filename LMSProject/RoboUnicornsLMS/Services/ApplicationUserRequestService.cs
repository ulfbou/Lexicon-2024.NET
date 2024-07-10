using LMS.api.Model;
using Microsoft.AspNetCore.Mvc;

namespace RoboUnicornsLMS.Services
{
    public class ApplicationUserRequestService : GenericRequestService<ApplicationUser, string>, IApplicationUserRequestService
    {
        public ApplicationUserRequestService(HttpClient httpClient, IConfiguration configuration, string serviceName)
            : base(httpClient, configuration, serviceName)
        { }

        // GET: /endpoint/{userId}/roles
        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId, bool includeRoles = true, CancellationToken cancellation = default)
        {
            try
            {
                var queryString = includeRoles ? "?includeRoles=true" : string.Empty;
                var uri = $"{_endpointPath}/{userId}/roles{queryString}";
                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                return result ?? Enumerable.Empty<string>();
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

        // POST: /endpoint/{userId}/roles
        public async Task<HttpResponseMessage> AddUserRoleAsync(string userId, string role, CancellationToken cancellation = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_endpointPath}/{userId}/roles", role);
                response.EnsureSuccessStatusCode();
                return response;
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

        // DELETE: /endpoint/{userId}/roles/{role}
        public async Task<HttpResponseMessage> RemoveUserRoleAsync(string userId, string role, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_endpointPath}/{userId}/roles/{role}");
                response.EnsureSuccessStatusCode();
                return response;
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
