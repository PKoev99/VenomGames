using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Generic repository for CRUD operations.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext context;
        private readonly DbSet<T> dbSet;

        public Repository(ApplicationDbContext _context)
        {
            context = _context;
            dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Retrieves all entities of type T.
        /// </summary>

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a single entity by its ID.
        /// </summary>
        async Task<T> IRepository<T>.GetByIdAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity of type {typeof(T).Name} with ID {id} was not found.");
            }
            else
            {
                return entity;
            }
        }

        /// <summary>
        /// Adds a new entity to the context.
        /// </summary>
        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

    }
}
