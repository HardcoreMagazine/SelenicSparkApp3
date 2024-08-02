using System.ComponentModel.DataAnnotations;

namespace dummyWebApi2.Models.Data
{
    public class CustomItem
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public CustomItem(string name)
        {
            Name = name;
        }

        public CustomItem(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
