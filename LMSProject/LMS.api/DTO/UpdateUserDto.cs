namespace LMS.api.DTO
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? CourseId { get; set; }
    }
}