using Generics.Models;
using UserService.Models.Data;

namespace UserService.Services.Data
{
    public class UserRolesManager : IRepository<UserRole>
    {
        public Task<int> CreateAsync(UserRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<UserRole>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserRole?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(UserRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
