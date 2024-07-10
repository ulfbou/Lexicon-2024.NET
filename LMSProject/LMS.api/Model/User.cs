using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS.api.Model
{
    public class User : IdentityUser, IEntity<string>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public string Name => $"{FirstName.ToUpperInvariant()} {LastName.ToUpperInvariant()}";
        public string SearchableString => $"{Name.ToUpperInvariant()} {Id}";

        // Navigation properties
        public int? CourseID { get; set; }
        [JsonIgnore]
        public Course? Course { get; set; }
    }
}
