using BusinessSuite.Models.Master_Models.enums;
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

        // Use the enum instead of string
        public string TransitCarrier { get; set; }

        public DateTime ScheduleTime { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

}
