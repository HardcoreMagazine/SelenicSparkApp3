using Generics.Models;
using Microsoft.EntityFrameworkCore;
using PostsService.Data;
using PostsService.Models.Data;

namespace PostsService.Service
{
    public class PostManager : IRepository<Post>
    {
        private readonly AppDbContext _appDbContext;

        public PostManager(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        /// <summary>
        /// Creates a Post entity in the database. Also saves changes/commits transaction.
        /// </summary>
        /// <param name="entity">Post object</param>
        /// <returns>Database object ID</returns>
        public async Task<int> CreateAsync(Post entity)
        {
            await _appDbContext.Posts.AddAsync(entity);
            await SaveChangesAsync();
            return entity.ID; // ID is returned on entity insertion
        }


        public async Task<IReadOnlyCollection<Post>> GetAllAsync()
        {
            return await _appDbContext.Posts
                .Where(x => x.Enabled)
                .OrderBy(x => x.ID)
                .ToListAsync();
        }

        public async Task<Post?> GetAsync(int id)
        {
            return await _appDbContext.Posts
                .FirstOrDefaultAsync(x => x.ID == id && x.Enabled);
        }

        /// <summary>
        /// Checks if Post object exists by ID, copies Post.Text from <paramref name="entity"/> 
        /// into the old object, updates and saves database change.
        /// </summary>
        /// <param name="entity">Post object</param>
        /// <returns>True on success</returns>
        public async Task<bool> UpdateAsync(Post entity)
        {
            var oldEntity = await GetAsync(entity.ID);
            if (oldEntity != null)
            {
                oldEntity.Text = entity.Text;
                _appDbContext.Posts.Update(oldEntity);
                await SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reads Post object by ID, changes Post.Enabled to 'false', updates and saves database change
        /// </summary>
        /// <param name="id">Post object ID</param>
        /// <returns>True on success</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                entity.Enabled = false;
                _appDbContext.Posts.Update(entity);
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
