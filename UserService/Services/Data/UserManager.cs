using Generics.Models;
using Microsoft.EntityFrameworkCore;
using System;
using UserService.Data;
using UserService.Models.Data;
using UserService.Models.SharedDictionary;

namespace UserService.Services.Data
{
    public class UserManager : IRepository<User>
    {
        private readonly AppDbContext _appDbContext;
        
        public UserManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Register new user IF selected email and username is not in use
        /// </summary>
        /// <returns>Fixed response code, see: <see cref="UserRegisterResponses"/></returns>
        public async Task<int> CreateAsync(User entity)
        {
            var userByName = await GetByUsernameAsync(entity.Username);
            if (userByName != null)
            {
                return (int)UserRegisterResponses.UsernameInUse;
            }

            var userByEmail = await GetByEmailAsync(entity.Email);
            if (userByEmail != null)
            {
                return (int)UserRegisterResponses.EmailInUse;
            }
            
            await _appDbContext.Users.AddAsync(entity);
            await SaveChangesAsync();

            return (int)UserRegisterResponses.Success;
        }

        public async Task<IReadOnlyCollection<User>> GetAllAsync()
        {
            return await _appDbContext.Users
                .Where(x => x.Enabled)
                .OrderBy(x => x.ID)
                .ToListAsync();
        }

        public async Task<User?> GetAsync(int id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.ID == id && x.Enabled);
        }

        public async Task<User?> GetByPublicIdAsync(string publicID)
        {
            var guid = Guid.Parse(publicID);
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.PublicID == guid && x.Enabled);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            email = email.ToLower();
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.Enabled);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Username == username && x.Enabled);
        }

        /// <summary>
        /// Updates user's username and email. 
        /// Important Note: these changes must be separated on frontend part
        /// </summary>
        /// <returns>True on success</returns>
        public async Task<bool> UpdateAsync(User entity)
        {
            var existingUser = await GetByPublicIdAsync(entity.PublicID.ToString());
            if (existingUser != null)
            {
                existingUser.Username = entity.Username;
                existingUser.Email = entity.Email;

                _appDbContext.Users.Update(existingUser);
                await _appDbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdatePasswordAsync(string publicID, string oldPassword, string newPassword)
        {
            var user = await GetByPublicIdAsync(publicID);
            if (user != null && BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                _appDbContext.Update(user);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This method is obsolete, use <see cref="DeleteByPublicID"/> instead
        /// </summary>
        [Obsolete("This method doesn't perform any validation/checks, use DeleteByPublicID instead")]
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetAsync(id);
            if (user != null)
            {
                user.Enabled = false;
                _appDbContext.Users.Update(user);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            else
        {
                return false;
            }
        }

        /// <summary>
        /// Disables user without deleting database entry, uses password validation/check
        /// </summary>
        /// <returns>True on success</returns>
        public async Task<bool> DeleteByPublicID(string publicID, string password)
        {
            var user = await GetByPublicIdAsync(publicID);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                user.Enabled = false;
                _appDbContext.Update(user);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
