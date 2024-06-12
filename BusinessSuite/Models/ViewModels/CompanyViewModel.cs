using X.PagedList;

namespace BusinessSuite.Models.ViewModels
{
    public class CompanyViewModel
    {
        public IPagedList<Company> companies { get; set; }
        public int totalpages { get; set; }
    }
}
