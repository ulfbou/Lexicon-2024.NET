using TenantManagement.Entities;

namespace TenantManagement.DTOs
{
    public class UpdateCourseDto : IIdentifyable
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; internal set; }
        public string Description { get; internal set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}