using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models.Master_Models
{
    public class ColumnMaster
    {
        [Key]
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool Visibility { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("TableMaster")]
        public int TableMasterId { get; set; }
        public TableMaster TableMasters { get; set; }
    }
}
