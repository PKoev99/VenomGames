namespace VenomGames.Core.Contracts
{
    /// <summary>
    /// Generic repository interface for CRUD operations.
    /// </summary>
    /// <typeparam name="T">Type of the entity.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities of type T.
        /// </summary>
        /// <returns>List of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity of type T by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>A single entity.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new entity of type T to the database.
        /// </summary>
        /// <param name="entity">Entity to be added.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity of type T.
        /// </summary>
        /// <param name="entity">Entity to be updated.</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity of type T by its ID.
        /// </summary>
        /// <param name="id">ID of the entity to be deleted.</param>
        Task DeleteAsync(int id);
    }
}
