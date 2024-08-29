using BusinessSuite.Data;
using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

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
            var userxs = _userManager.Users.ToList();
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
                //companyViewModel.Company = company.CompanyName;
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


        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IdentityUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null) return NotFound();

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded) return RedirectToAction(nameof(Index));

            // Handle errors
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return RedirectToAction(nameof(Index));

            // Handle errors
            return View(user);
        }
    }
}
