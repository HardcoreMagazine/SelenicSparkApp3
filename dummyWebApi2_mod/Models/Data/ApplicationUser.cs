using System.ComponentModel.DataAnnotations;

namespace dummyWebApi2.Models.Data
{
    public class ApplicationUser
    {
        [Key]
        public int ID { get; set; }
        public Guid PublicID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public bool Enabled { get; set; } = true;
        public DateTimeOffset DateJoin { get; set; } = DateTimeOffset.Now;

        // you don't need to use 'id' in here if you're using some external database like PostgreSQL
        public ApplicationUser(int id, string name, string email, string password)
        {
            ID = id;
            Name = name;
            Email = email;
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
