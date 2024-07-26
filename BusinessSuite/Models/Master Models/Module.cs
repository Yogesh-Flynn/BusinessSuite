using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSuite.Models.Master_Models
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        [ForeignKey("Website")]
        public int WebsiteId { get; set; }
        public Website Websites { get; set; }
    }
}
