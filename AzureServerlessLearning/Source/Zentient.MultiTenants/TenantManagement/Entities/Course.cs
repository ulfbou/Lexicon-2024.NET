using Azure.Data.Tables;

namespace TenantManagement.Entities
{
    public class Course : TenantEntity, ITableEntity, IIdentifyable
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
        public string Title { get; internal set; }
        public string Description { get; internal set; }
        public List<Announcement> Announcements { get; set; }
        public List<Grade> Grades { get; set; }
        public List<User> Students { get; set; }
        public List<User> Teachers { get; set; }
        public List<Resource> Resources { get; set; }
        public string Id => RowKey;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}