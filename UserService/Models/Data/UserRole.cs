using Generics.Models;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Data
{
    public class UserRole : IEntity
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public Guid UserID { get; set; }
        public int RoleID { get; set; }
    }
}
