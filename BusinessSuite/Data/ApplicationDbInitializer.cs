using BusinessSuite.Models;
using Microsoft.AspNetCore.Identity;

namespace BusinessSuite.Data
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var powerUser = new ApplicationUser
            {
                UserName = "admin@domain.com",
                Email = "admin@domain.com",
                FirstName = "SuperAdmin",
                LastName = "Test", 
            };

            string userPassword = "Admin@123";
            var user = await userManager.FindByEmailAsync("admin@domain.com");

            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(powerUser, "Admin");
                }
            }



        }
    }
}
