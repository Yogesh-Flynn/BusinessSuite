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
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // Number of items per page
            int pageNumber = (page ?? 1);
            var users = await _userManager.Users.ToListAsync();
            var appusers = await _dbcontext.ApplicationUsers.ToListAsync();
            
            List<UserRoleCompanyViewModel> userRoleCompanyViewModel = new List<UserRoleCompanyViewModel>();
            for (int i = 0; i < 100; i++)
            {
                foreach (var user in appusers)
                {
                    UserRoleCompanyViewModel companyViewModel = new UserRoleCompanyViewModel();

                    companyViewModel.FirstName = user.FirstName+i.ToString();
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

            }
            var pagedUsers = userRoleCompanyViewModel.ToPagedList(pageNumber, pageSize);

            return View(pagedUsers);
           // return View(userRoleCompanyViewModel);
        }
    }
}
