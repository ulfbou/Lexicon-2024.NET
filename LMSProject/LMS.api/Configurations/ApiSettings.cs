namespace LMS.api.Configurations
{
    public class ApiSettings
    {
        public string? Host { get; set; }
        public string? EndPoint { get; set; }
        public Dictionary<Type, string>? Mappings { get; set; }
    }
}
