using Generics.Models;

namespace UserService.Models.Data
{
    public interface IUserRepository<T> where T : IEntity
    {
        /// <summary>
        /// Creates new User record in the DB
        /// </summary>
        /// <returns>Null on success, user-readable String on fail</returns>
        Task<string?> RegisterAsync(string username, string email, string password);
        /// <summary>
        /// Authenticates user if login (email) and password is correct
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>String-token on success, Null on fail</returns>
        Task<string?> LoginAsync(string email, string password);
        Task<IReadOnlyCollection<T>> GetAllUsersAsync();
        Task<T?> GetUserAsync(string publicID);
        Task<T?> GetUserByNameAsync(string name);
        Task<T?> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserUsernameAsync(string publicID, string password, string username);
        Task<bool> UpdateUserEmailAsync(string publicID, string password, string email);
        Task<bool> UpdateUserPasswordAsync(string publicID, string currentPassword, string newPassword);
        Task<bool> DeleteUserAsync(string publicID, string password);
        Task SaveChangesAsync();
    }
}
