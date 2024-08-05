using Generics.Models;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Data
{
    public class Role : IEntity
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }

        public static bool Validate(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
                return false;
            else
                return true;
        }
    }
}
