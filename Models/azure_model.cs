using System;

namespace Azure_week3.Models
{
    public class azure_model
    {
        public Guid RegistrationId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int Zipcode { get; set; }
        public string Age { get; set; }
        public bool IsFirstTimer { get; set; }
    }
}
