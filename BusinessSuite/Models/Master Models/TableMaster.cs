using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models.Master_Models
{
    public class TableMaster
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
      
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        [ForeignKey("DatabaseMaster")]
        public int DatabaseMasterId { get; set; }
        public DatabaseMaster DatabaseMasters { get; set; }

        public ICollection<ColumnMaster> ColumnMasters { get; set; }
    }
}
