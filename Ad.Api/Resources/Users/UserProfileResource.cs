using System;

namespace Ad.API.Resources.User
{
    public class UserProfileResource
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Gender { get; set; }
        public string HomeAddress { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        
    }
}