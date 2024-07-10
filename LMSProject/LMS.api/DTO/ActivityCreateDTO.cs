using System.ComponentModel.DataAnnotations;

namespace LMS.api.DTO
{
    public class ActivityCreateDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }
        
        [Required]
        public int ModuleID { get; set; }
    }
}
