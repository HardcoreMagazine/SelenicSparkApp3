using System.ComponentModel.DataAnnotations;

namespace dummyWebApi2.Models.Data
{
    public class UserRoles
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }

        public UserRoles(int id, int userID, int roleID)
        {
            ID = id;
            UserID = userID;
            RoleID = roleID;
        }
    }
}
