using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models.Data;
using UserService.Models.SharedDictionary;
using UserService.Services.Security;

namespace UserService.Services.Data
{
    public class UserManager : IUserRepository<User>
    {
        private readonly AppDbContext _appDbContext;
        
        public UserManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Register new user IF selected email and username is not in use
        /// </summary>
        /// <returns>Fixed response code, see: <see cref="EntityCreateResponses"/></returns>
        public async Task<string?> RegisterAsync(string username, string email, string password)
        {
            var userByName = await GetUserByNameAsync(username);
            if (userByName != null)
            {
                return "Selected Username is already in use";
            }

            var userByEmail = await GetUserByEmailAsync(email);
            if (userByEmail != null)
            {
                return "Selected Email is already in use";
            }

            var user = new User()
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email
            };

            await _appDbContext.Users.AddAsync(user);
            await SaveChangesAsync();

            return null;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                string token = JwtTokenManager.GenerateUserToken(user);
                return token;
            }
            else
            {
                return null;
            }
        }

        public async Task<IReadOnlyCollection<User>> GetAllUsersAsync()
        {
            return await _appDbContext.Users
                .Where(x => x.Enabled)
                .OrderBy(x => x.ID)
                .ToListAsync();
        }

        public async Task<User?> GetUserAsync(string publicID)
        {
            var guid = Guid.Parse(publicID);
            return await _appDbContext.Users
                .FirstOrDefaultAsync(x => x.PublicID == guid && x.Enabled);
        }

        public async Task<User?> GetUserByNameAsync(string username)
        {
            return await _appDbContext.Users
                .FirstOrDefaultAsync(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.Enabled);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            email = email.ToLower();
            return await _appDbContext.Users
                .FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) && x.Enabled);
        }

        public async Task<bool> UpdateUserUsernameAsync(string publicID, string password, string username)
        {
            var user = await GetUserAsync(publicID);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                user.Username = username;
                _appDbContext.Users.Update(user);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserEmailAsync(string publicID, string password, string email)
        {
            var user = await GetUserAsync(publicID);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                user.Email = email.ToLower();
                _appDbContext.Users.Update(user);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserPasswordAsync(string publicID, string currentPassword, string newPassword)
        {
            var user = await GetUserAsync(publicID);
            if (user != null && BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
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
        /// Disables User without deleting database entry, uses password validation/check
        /// </summary>
        /// <returns>True on success</returns>
        public async Task<bool> DeleteUserAsync(string publicID, string password)
        {
            var user = await GetUserAsync(publicID);
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

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
