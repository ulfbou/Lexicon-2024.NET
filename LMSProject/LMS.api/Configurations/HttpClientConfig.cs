namespace LMS.api.Configurations
{
    public class HttpClientConfig
    {
        public string? BaseAddress { get; set; }
        public Dictionary<string, string>? DefaultRequestHeaders { get; set; }
        public string? EndpointPath { get; set; }
    }
}