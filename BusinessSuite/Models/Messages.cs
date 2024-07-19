using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public DateTime ScheduleTime { get; set; } = DateTime.Now;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
