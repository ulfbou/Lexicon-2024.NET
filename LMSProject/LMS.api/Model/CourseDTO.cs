namespace LMS.api.Model
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int maxCapcity { get; set; } //I spelled it wrong intentionally based on the schema. /Martin 
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;

    }
    public class CourseCreateDTO
    {
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int MaxCapacity { get; set; } //I spelled it wrong intentionally based on the schema. /Martin 
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;

    }
    public class CourseUpdateDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int MaxCapacity { get; set; } //I spelled it wrong intentionally based on the schema. /Martin 
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;

    }
}
