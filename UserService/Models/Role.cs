namespace UserService.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }

        public static bool Validate (Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
                return false;
            else
                return true;
        }
    }
}
