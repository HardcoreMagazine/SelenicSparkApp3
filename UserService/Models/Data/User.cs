using Generics.Models;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Data
{
    public class User : IEntity
    {
        private const int MinUsernameLen = 2;
        private const int MaxUsernameLen = 96;

        [Key]
        public int ID { get; set; }
        [Key]
        public Guid PublicID { get; set; } = Guid.NewGuid();
        [StringLength(maximumLength: MaxUsernameLen, MinimumLength = MinUsernameLen)]
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public DateTimeOffset DateJoin { get; set; } = DateTimeOffset.Now;
        public bool Enabled { get; set; } = true;

        public static bool Validate(User user)
        {
            return !string.IsNullOrWhiteSpace(user.Email)
                && !string.IsNullOrWhiteSpace(user.Username)
                && !string.IsNullOrWhiteSpace(user.PasswordHash);
        }
    }
}
