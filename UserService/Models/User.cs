namespace UserService.Models
{
    public class User
    {
        public Guid GUID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // is unsafe; will hash && salt later
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public bool PhoneAuthenticatorEnabled { get; set; } = false;
    }
}
