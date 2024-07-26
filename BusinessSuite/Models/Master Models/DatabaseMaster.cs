using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models.Master_Models
{
    public class DatabaseMaster
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        [ForeignKey("Website")]
        public int WebsiteId { get; set; }
        public Website Websites { get; set; }

        public ICollection<TableMaster> TableMasters { get; set; }
    }
}
