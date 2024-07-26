using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string MessageText { get; set; }
        public string? Image { get; set; }
        public DateTime ScheduleTime { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public bool IsDeleted { get; set; }=false;
    }
}
