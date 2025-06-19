using VEROSA.Common.Enums;

namespace VEROSA.Common.Models.Request
{
    public class AccountRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public AccountRole Role { get; set; }

        public AccountStatus Status { get; set; }
    }
}
