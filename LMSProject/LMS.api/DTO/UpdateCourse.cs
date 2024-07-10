namespace LMS.api.DTO
{
    public class UpdateCourseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public int? MaxCapacity { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
    }
}
