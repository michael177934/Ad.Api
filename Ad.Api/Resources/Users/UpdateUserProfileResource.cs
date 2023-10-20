using Microsoft.AspNetCore.Http;
using System;

namespace Ad.API.Resources.User
{
    public class UpdateUserProfileResource
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string HomeAddress { get; set; }
        public string OfficeAddress { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BVN { get; set; }
        public string CustomerId { get; set; }
        public IFormFile ProfilePicFile { get; set; }
        public IFormFile ValidIdFile { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmployeerEmailAddress { get; set; }
        public string EmployeerPhoneNumber { get; set; }
        public string EmployeerAddress { get; set; }
        public string CompanyName { get; set; }
        public string NextOfKinFirstName { get; set; }
        public string NextOfKinLastName { get; set; }
        public string NextOfKinAddress { get; set; }
        public string NextOfKinPhoneNumber { get; set; }
        public bool? IsLoanCurrentlyDeductedFromSalary { get; set; }
        public decimal? NetMonthlySalary { get; set; }
    }
}