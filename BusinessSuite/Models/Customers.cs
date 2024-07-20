using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Domain { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public ICollection<Marketing_Customers> Marketing_Customers { get; set; }

    }
}
