using System.Data;

namespace BusinessSuite.Models.ViewModels
{
    public class DisplayTableViewModel
    {
        public List<string> DisplayTables { get; set; }
        public DataTable ColumnSchema { get; set; }
    }
}
