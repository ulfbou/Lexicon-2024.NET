namespace LMS.api.DTO
{
    public class CreateUserDto
    {
        public string UserName { get; internal set; }
        public string Email { get; internal set; }
        public string Password { get; internal set; }
    }
}