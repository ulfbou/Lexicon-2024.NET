using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS.api.Model
{
    public class Module : IDatableEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public DateTime Start { get; set; } = DateTime.Today;
        public DateTime End { get; set; } = DateTime.Today.AddMonths(1);
        public string Title { get; set; }
        public string Description { get; set; }

        public int CourseID { get; set; }

        [JsonIgnore]
        public Course Course { get; set; }

        [JsonIgnore]
        public ICollection<Activity> Activities { get; set; }

        public string SearchableString => $"{Title} {Id}";
    }
}
