using dummyWebApi2.Models.Data;

namespace dummyWebApi2.Services
{
    public class RoleManager
    {
        private static readonly List<ApplicationRole> ApplicationRolesRepo = new()
        {
            new(1, "Admin"),
            new(2, "User")
        };

        private static readonly List<UserRoles> UserRolesRepo = new()
        {
            new(1, 1, 1),
            new(2, 2, 2),
            new(2, 3, 2)
        };

        public static List<ApplicationRole> GetRoles()
        {
            return ApplicationRolesRepo;
        }

        public static List<string> GetUserRoles(int id)
        {
            var userRoles = UserRolesRepo
                .Where(ur => ur.UserID == id)
                .Join(ApplicationRolesRepo, ur => ur.RoleID, r => r.ID, (ur, r) => r.Name)
                .ToList();
            return userRoles;
        }

        public static bool GrandRole(int roleID, int userID)
        {
            if (UserRolesRepo.Any(x => x.RoleID == roleID && x.UserID == userID))
            {
                return false;
            }
            else
            {
                var nextMaxID = UserRolesRepo.Max(x => x.ID) + 1;
                UserRolesRepo.Add(new(nextMaxID, userID, roleID));
                return true;
            }
        }

        public static bool RevokeRole(int roleID, int userID)
        {
            var userRole = UserRolesRepo.FirstOrDefault(x => x.RoleID == roleID && x.UserID == userID);
            if (userRole == null)
            {
                return false;
            }
            else
            {
                UserRolesRepo.Remove(userRole);
                return true;
            }
        }

        public static bool CreateRole(string name)
        {
            if (ApplicationRolesRepo.Any(x => x.Name == name))
            {
                return false;
            }
            else
            {
                var nextMaxID = ApplicationRolesRepo.Max(x => x.ID) + 1;
                ApplicationRolesRepo.Add(new(nextMaxID, name));
                return true;
            }
        }

        public static bool DeleteRoleCascade(string name)
        {
            var role = ApplicationRolesRepo.FirstOrDefault(x => x.Name == name);
            if (role == null)
            {
                return false;
            }
            else
            {
                UserRolesRepo.RemoveAll(ur => ur.RoleID == role.ID);
                ApplicationRolesRepo.Remove(role);
                return true;
            }
        }
    }
}
