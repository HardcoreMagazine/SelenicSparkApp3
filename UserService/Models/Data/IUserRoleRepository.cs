using Generics.Models;

namespace UserService.Models.Data
{
    public interface IUserRoleRepository<T> where T : IEntity
    {
        Task<bool> GrantRoleToUserAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllUserRolesAsync(string publicID);
        Task<T?> GetUserInRoleAsync(string publicID, string role);
        Task<T?> GetUserInRoleAsync(string publicID, int roleID);
        Task<bool> UserIsInRoleAsync(string publicID, string role);
        Task<bool> UserIsInRoleAsync(string publicID, int roleID);
        Task<bool> RevokeRoleFromUserAsync(string publicID, string role);
        Task SaveChangesAsync();
    }
}
