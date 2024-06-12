using BusinessSuite.Data;
using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BusinessSuite.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbcontext;

        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }
        public async Task<IActionResult> GetUsers(int? page)
        {
            int pageSize = 1000; // Number of items per page
            int pageNumber = (page ?? 1);
            var users = await _userManager.Users.ToListAsync();
            var appusers = await _dbcontext.ApplicationUsers.ToListAsync();
            UserTableViewModel userTableViewModel = new UserTableViewModel();
            List<UserRoleCompanyViewModel> userRoleCompanyViewModel = new List<UserRoleCompanyViewModel>();
            
            foreach (var user in appusers)
            {
                UserRoleCompanyViewModel companyViewModel = new UserRoleCompanyViewModel();

                companyViewModel.FirstName = user.FirstName.ToString();
                companyViewModel.LastName = user.LastName;
                companyViewModel.Email = user.Email;
                companyViewModel.Phone = user.PhoneNumber;
                companyViewModel.UserName = user.UserName;

                var company = await _dbcontext.Companies.Where(x => x.Id == user.CompanyId).FirstOrDefaultAsync();
                companyViewModel.Company = company.CompanyName;
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    companyViewModel.Role += role;
                }

                userRoleCompanyViewModel.Add(companyViewModel);
            }

            
            var pagedUsers = userRoleCompanyViewModel.ToPagedList(pageNumber, pageSize);
            userTableViewModel.totalpages = userRoleCompanyViewModel.Count/ pageSize;
            userTableViewModel.userRoleCompanyViewModels=pagedUsers;
            return View(userTableViewModel);
           
        }
        public async Task<IActionResult> GetCompanies(int? page)
        {
            int pageSize = 10; // Number of items per page
            int pageNumber = (page ?? 1);
            var appusers = await _dbcontext.Companies.ToListAsync();
            CompanyViewModel companyView = new CompanyViewModel();
            
            var pagedUsers = appusers.ToPagedList(pageNumber, pageSize);
            companyView.totalpages = appusers.Count/ pageSize;
            companyView.companies = pagedUsers;
            return View(companyView);
          
        }
    }
}
