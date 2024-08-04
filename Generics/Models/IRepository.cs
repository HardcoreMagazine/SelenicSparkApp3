namespace Generics.Models
{
    /// <summary>
    /// Generalized entity repository with basic CRUD actions
    /// </summary>
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Creates entity in the database
        /// </summary>
        /// <param name="entity">IEntity child object</param>
        /// <returns>ID: int of created entity</returns>
        Task<int> CreateAsync(T entity);

        /// <summary>
        /// Lists all entities in the database
        /// </summary>
        /// <returns>Collection of IEntity children objects</returns>
        Task<IReadOnlyCollection<T>> GetAllAsync();

        /// <summary>
        /// Lists properties of one specific entity by ID
        /// </summary>
        /// <param name="id">IEntity child object ID</param>
        /// <returns>IEntity child object</returns>
        Task<T?> GetAsync(int id);

        /// <summary>
        /// Deletes one specific entity by ID
        /// </summary>
        /// <param name="id">IEntity child object ID</param>
        /// <returns>True on successful removal</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Updates properties of one specific entity
        /// </summary>
        /// <param name="entity">IEntity child object</param>
        /// <returns>True on successful update</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Commits changes made to the database entities
        /// </summary>
        Task SaveChangesAsync();
    }
}
