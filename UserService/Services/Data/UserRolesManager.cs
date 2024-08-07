using Generics.Models;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models.Data;
using UserService.Models.SharedDictionary;

namespace UserService.Services.Data
{
    public class UserRolesManager : IRepository<UserRole>
    {
        private readonly AppDbContext _appDbContext;

        public UserRolesManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> CreateAsync(UserRole entity)
        {
            var ur = await _appDbContext.UserRoles
                .FirstOrDefaultAsync(x => x.RoleID == entity.RoleID && x.UserID == entity.UserID);
            if (ur == null)
            {
                await _appDbContext.UserRoles.AddAsync(entity);
                await SaveChangesAsync();
                return (int)EntityCreateResponses.Success;
            }
            else
            {
                return (int)EntityCreateResponses.Exists;
            }
        }

        public async Task<IReadOnlyCollection<UserRole>> GetAllAsync()
        {
            return await _appDbContext.UserRoles.ToListAsync();
        }

        /// <summary>
        /// This method won't be implemented, use <see cref="GetUserInRoleAsync"/> or <see cref="UserIsInRoleAsync"/> instead
        /// </summary>
        /// <returns>Always null!</returns>
        [Obsolete("Won't be implemented, use methods GetUserInRoleAsync or UserIsInRoleAsync instead")]
        public async Task<UserRole?> GetAsync(int id)
        {
            await Task.Run(()=>null); // dummy
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets UserRole objects by user publicID and role name
        /// </summary>
        public async Task<UserRole?> GetUserInRoleAsync(string publicID, string role)
        {
            // we do this the easy way
            var roleID = (await _appDbContext.Roles
                .FirstOrDefaultAsync(r => r.Name.Equals(role, StringComparison.InvariantCultureIgnoreCase)))
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

        /// <summary>
        /// Won't be implemented, use <see cref=""/> and <see cref="CreateAsync"/> instead
        /// </summary>
        [Obsolete("Won't be implemented, use Delete/Create functions instead")]
        public async Task<bool> UpdateAsync(UserRole entity)
        {
            await Task.Run(() => null); // dummy
            throw new NotImplementedException();
        }

        /// <summary>
        /// Won't be implemented, use <see cref="DeleteAsync"/> instead
        /// </summary>
        [Obsolete("Won't be implemened, use DeleteRoleFromUserAsync instead")]
        public async Task<bool> DeleteAsync(int id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes user from selected role
        /// </summary>
        /// <param name="publicID">User Public ID (GUID)</param>
        /// <param name="role">Role name</param>
        /// <returns>True on success</returns>
        public async Task<bool> DeleteRoleFromUserAsync(string publicID, string role)
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

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
