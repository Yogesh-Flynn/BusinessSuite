using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSuite.Models.Master_Models
{
    public class MasterUICodes
    {
        [Key]
        public int Id {  get; set; }
        public string PageName { get; set; }
        public string PageCode { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ForeignKey("Website")]
        public int WebsiteId { get; set; }
        public Website Websites { get; set; }
    }
}
