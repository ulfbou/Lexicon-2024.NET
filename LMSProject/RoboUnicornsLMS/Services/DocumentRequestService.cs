using LMS.api.Model;

namespace RoboUnicornsLMS.Services
{
    public class DocumentRequestService : GenericRequestService<Document>, IDocumentRequestService
    {
        public DocumentRequestService(HttpClient httpClient, IConfiguration configuration, string serviceName)
            : base(httpClient, configuration, serviceName)
        { }
    }
}
