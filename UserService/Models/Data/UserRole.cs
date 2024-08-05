using Generics.Models;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Data
{
    public class UserRole : IEntity
    {
        [Key]
        public int ID { get; set; }
        public Guid UsertID { get; set; }
        public int RoleID { get; set; }
    }
}
