using Generics.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using UserService.Data;
using UserService.Models.Data;
using UserService.Models.SharedDictionary;

namespace UserService.Services.Data
{
    public class RoleManager : IRepository<Role>
    {
        private readonly AppDbContext _appDbContext;

        public RoleManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> CreateAsync(Role entity)
        {
            var roleExists = await GetByNameAsync(entity.Name);
            if (roleExists == null)
            {
                _appDbContext.Roles.Add(entity);
                await SaveChangesAsync();
                return (int)EntityCreateResponses.Success;
            }
            else
            {
                return (int)EntityCreateResponses.Exists;
            }
        }

        public async Task<IReadOnlyCollection<Role>> GetAllAsync()
        {
            return await _appDbContext.Roles
                .Where(r => r.Enabled)
                .OrderBy(r => r.ID)
                .ToListAsync();
        }

        public async Task<Role?> GetAsync(int id)
        {
            return await _appDbContext.Roles.FirstOrDefaultAsync(r => r.ID == id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _appDbContext.Roles
                .FirstOrDefaultAsync(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Updates role name. Does not validate Role.Name property
        /// </summary>
        /// <returns>True on success</returns>
        public async Task<bool> UpdateAsync(Role entity)
        {
            var oldRole = await GetAsync(entity.ID);
            if (oldRole != null)
            {
                oldRole.Name = entity.Name;
                _appDbContext.Roles.Update(oldRole);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Disables role without deleting database entry
        /// </summary>
        /// <returns>True on success</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var role = await GetAsync(id);
            if (role != null)
            {
                role.Enabled = false;
                _appDbContext.Roles.Update(role);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Similar to <see cref="DeleteAsync"/> - disables role by name without deleting database entry
        /// </summary>
        /// <returns>True on success</returns>
        public async Task<bool> DeleteByNameAsync(string name)
        {
            var role = await GetByNameAsync(name);
            if (role != null)
            {
                role.Enabled = false;
                _appDbContext.Roles.Update(role);
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
