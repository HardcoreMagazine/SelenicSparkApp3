using Generics.Models;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Data
{
    public class Role : IEntity
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
