using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace BusinessSuite.Models.ViewModels
{
   

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Organization")]
        public string CompanyName { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
        [BindNever]  // This will prevent model binding and avoid validation issues
        public List<SelectListItem> RoleList { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile ProfilePhoto { get; set; } // Used to handle file uploads
    }

}
