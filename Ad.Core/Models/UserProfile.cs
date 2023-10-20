using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Core.Models
{
    public class UserProfile
    {
        public long Id { get; set; }
        public string ProfileId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string HomeAddress { get; set; }
        public string EmployeerAddress { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public Guid TenantId { get; set; }
        public ApplicationUser Profile { get; set; }
    }
}
