using Microsoft.AspNetCore.Identity;

namespace BusinessSuite.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
