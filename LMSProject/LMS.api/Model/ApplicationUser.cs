using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace LMS.api.Model
{
    public class ApplicationUser : User, IEntity<string>
    {
        public string Password
        {
            get => PasswordHash ?? string.Empty;
            set => PasswordHash = value ?? throw new ArgumentNullException(nameof(Password));
        }

        public ICollection<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
    }
}
