using LMS.api.Model;

namespace RoboUnicornsLMS.Services
{
    public class CourseRequestService : GenericRequestService<Course>, ICourseRequestService
    {
        public CourseRequestService(HttpClient httpClient, IConfiguration configuration, string serviceName)
            : base(httpClient, configuration, serviceName)
        { }

        public async Task<Course?> GetCourseForUserAsync(string userId, CancellationToken cancellation = default)
        {
            return await _httpClient.GetFromJsonAsync<Course>($"{_endpointPath}/{userId}");
        }

        public Task<HttpResponseMessage> UpdateAsync(int courseId, CourseDTO entity, CancellationToken cancellation = default)
        {
            return _httpClient.PutAsJsonAsync($"{_endpointPath}/{courseId}", entity);
        }
    }
}
