using System;

namespace Ad.API.Resources.User
{
    public class UserResource
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDisabled { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
        public string Role { get; set; }
    }

}