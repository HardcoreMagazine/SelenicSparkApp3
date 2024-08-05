using Generics.Models;
using UserService.Models.Data;

namespace UserService.Services.Data
{
    public class RoleManager : IRepository<Role>
    {
        public Task<int> CreateAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Role>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Role?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}
