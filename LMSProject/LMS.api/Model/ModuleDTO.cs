namespace LMS.api.Model
{
    public class ModuleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int CourseId { get; set; }

        public Course? Course { get; set; }

        public ICollection<Activity>? Activities { get; set; }
    }
}
