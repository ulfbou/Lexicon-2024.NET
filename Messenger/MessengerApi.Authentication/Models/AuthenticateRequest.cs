using System.ComponentModel;

namespace MessengerAPI.Authentication.Models
{
    public class AuthenticateRequest
    {
        [DefaultValue("NoName")]
        public required string Username { get; set; }

        [DefaultValue("NoPwd")]
        public required string Password { get; set; }
    }
}
