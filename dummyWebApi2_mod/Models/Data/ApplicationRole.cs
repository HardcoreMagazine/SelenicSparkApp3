using System.ComponentModel.DataAnnotations;

namespace dummyWebApi2.Models.Data
{
    public class ApplicationRole
    {
        [Key]
        public int ID { get; private set; }
        public string Name { get; set; }

        public ApplicationRole(string name)
        {
            Name = name;
        }

        public ApplicationRole(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
