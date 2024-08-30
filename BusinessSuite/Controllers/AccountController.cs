using Microsoft.AspNetCore.Mvc;

namespace BusinessSuite.Controllers
{
    using BusinessSuite.Models.ViewModels;
    using BusinessSuite.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            var model = new RegisterViewModel
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToList()
            };
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            // Remove specific entries from ModelState
            if (ModelState.ContainsKey("RoleList"))
            {
                ModelState.Remove("RoleList");
            }

            // Populate RoleList manually
            model.RoleList = _roleManager.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToList();
            string ProfilePhotoBase64 = "";
            // Handle file upload
            if (model.ProfilePhoto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ProfilePhoto.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    ProfilePhotoBase64 = Convert.ToBase64String(fileBytes);
                }
            }

            ViewData["ReturnUrl"] = returnUrl;

            // Check if the email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    ProfilePhotoBase64 = ProfilePhotoBase64 // Store the base64 string in the user object
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}
