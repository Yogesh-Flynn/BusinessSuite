using X.PagedList;

namespace BusinessSuite.Models.ViewModels
{
    public class UserTableViewModel
    {
        public IPagedList<UserRoleCompanyViewModel> userRoleCompanyViewModels{ get; set; }
        public int totalpages { get; set; }
    }
}
