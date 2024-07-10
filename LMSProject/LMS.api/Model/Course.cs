using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS.api.Model
{
    public class Course : IDatableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The title is required.")]
        [MaxLength(100, ErrorMessage = "The title must be 100 characters or less.")]
        [MinLength(3, ErrorMessage = "The title must be at least 3 characters.")]
        public string Title { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The description is required.")]
        [MaxLength(500, ErrorMessage = "The description must be 500 characters or less.")]
        [MinLength(3, ErrorMessage = "The description must be at least 3 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "The start date is required.")]
        public DateTime Start { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "The end date is required.")]
        public DateTime End { get; set; } = DateTime.Today.AddMonths(1);

        [Required(ErrorMessage = "The maximum capacity is required.")]
        [Range(1, 100, ErrorMessage = "The maximum capacity must be between 1 and 100.")]
        public int MaxCapcity { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime Created { get; set; }

        public string SearchableString => $"{Title} {Id}";

        [JsonIgnore]
        public ICollection<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();

        [JsonIgnore]
        public ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}
