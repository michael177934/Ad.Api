using Microsoft.AspNetCore.Identity;

namespace Ad.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string BVN { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid TenantId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AccountNumber { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Balance { get; set; }
        

    }


    public class ApplicationRole : IdentityRole
    {
        public string CreatedBy { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Permissions { get; set; }
        public string Menus { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }
        public Guid TenantId { get; set; }
    }
}
