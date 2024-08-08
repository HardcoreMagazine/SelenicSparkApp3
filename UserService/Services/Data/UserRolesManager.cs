using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models.Data;

namespace UserService.Services.Data
{
    public class UserRolesManager : IUserRoleRepository<UserRole>
    {
        private readonly AppDbContext _appDbContext;

        public UserRolesManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Adds specific role to specific user; performs validations: 
        /// if role exists and if user is already in a role
        /// </summary>
        /// <returns>True on success</returns>
        public async Task<bool> GrantRoleToUserAsync(UserRole entity)
        {
            var roleExists = await _appDbContext.Roles
                .FirstOrDefaultAsync(x=> x.Enabled && x.ID == entity.RoleID);

            if (roleExists != null) 
                return false;

            var ur = await _appDbContext.UserRoles
                .FirstOrDefaultAsync(x => x.RoleID == entity.RoleID && x.UserID == entity.UserID);
            if (ur == null)
            {
                await _appDbContext.UserRoles.AddAsync(entity);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IReadOnlyCollection<UserRole>> GetAllAsync()
        {
            return await _appDbContext.UserRoles.ToListAsync();
        }

        public async Task<IReadOnlyCollection<UserRole>> GetAllUserRolesAsync(string publicID)
        {
            var guid = Guid.Parse(publicID);
            return await _appDbContext.UserRoles
                .Where(x => x.UserID == guid)
                .ToListAsync();
        }

        /// <summary>
        /// Gets UserRole objects by user publicID and role name
        /// </summary>
        public async Task<UserRole?> GetUserInRoleAsync(string publicID, string role)
        {
            // we do this the easy way
            var roleID = (await _appDbContext.Roles
                .FirstOrDefaultAsync(r => r.Enabled && r.Name.Equals(role, StringComparison.InvariantCultureIgnoreCase)))
                ?.ID;

            if (roleID != null)
            {
                var guid = Guid.Parse(publicID);
                
                var userRole = await _appDbContext.UserRoles
                    .FirstOrDefaultAsync(x => x.RoleID == roleID && x.UserID == guid);
                return userRole;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserRole?> GetUserInRoleAsync(string publicID, int roleID)
        {
            var guid = Guid.Parse(publicID);
            var userRole = await _appDbContext.UserRoles
                .FirstOrDefaultAsync(x => x.RoleID == roleID && x.UserID == guid);
            return userRole;
        }

        /// <summary>
        /// Checks if user is in specific role
        /// </summary>
        /// <param name="publicID">User Public ID (GUID)</param>
        /// <param name="role">Role name</param>
        /// <returns>True when user is in role</returns>
        public async Task<bool> UserIsInRoleAsync(string publicID, string role)
        {
            var userRole = await GetUserInRoleAsync(publicID, role);
            return userRole != null;
        }

        public async Task<bool> UserIsInRoleAsync(string publicID, int roleID)
        {
            var userRole = await GetUserInRoleAsync(publicID, roleID);
            return userRole != null;
        }

        /// <summary>
        /// Deletes user from selected role
        /// </summary>
        /// <param name="publicID">User Public ID (GUID)</param>
        /// <param name="role">Role name</param>
        /// <returns>True on success</returns>
        public async Task<bool> RevokeRoleFromUserAsync(string publicID, string role)
        {
            var userRole = await GetUserInRoleAsync(publicID, role);
            if (userRole != null)
            {
                _appDbContext.UserRoles.Remove(userRole);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RevokeRoleFromUserAsync(string publicID, int roleID)
        {
            var userRole = await GetUserInRoleAsync(publicID, roleID);
            if (userRole != null)
            {
                _appDbContext.UserRoles.Remove(userRole);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
