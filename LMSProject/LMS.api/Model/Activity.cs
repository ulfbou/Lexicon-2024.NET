using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS.api.Model
{
    public class Activity : IDatableEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public DateTime Start { get; set; } = DateTime.Today;
        public DateTime End { get; set; } = DateTime.Today.AddMonths(1);

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int ModuleID { get; set; }

        public string SearchableString => $"{Title} {Id}";

        [JsonIgnore]
        public Module Module { get; set; }
    }
}
