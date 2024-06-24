namespace UserService.Models
{
    public class User
    {
        public int ID { get; set; } // [should] only be visible to admins && own users
        public Guid GUID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // is unsafe; will hash && salt later
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public bool PhoneAuthenticatorEnabled { get; set; } = false;
        public bool Enabled { get; set; } = true;

        public static bool Validate(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Username) 
                || string.IsNullOrWhiteSpace(user.Password))
                return false;
            else
                return true;
        }
    }
}
