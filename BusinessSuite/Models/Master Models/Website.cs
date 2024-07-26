using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models.Master_Models
{
    public class Website
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<DatabaseMaster> DatabasesMaster { get; set; }
    }
}
