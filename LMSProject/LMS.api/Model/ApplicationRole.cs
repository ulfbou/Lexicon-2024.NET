using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization.Policy;

namespace LMS.api.Model
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }
        public
            ApplicationRole(string roleName) : base(roleName)
        {
        }

        public const string Admin = "ADMIN";
        public const string Teacher = "TEACHER";
        public const string Student = "STUDENT";

        public string SearchableString => Name.ToUpperInvariant();

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}