using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data;

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
        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        /// <summary>
        /// Retrieves a single entity by its ID.
        /// </summary>
        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Adds a new entity to the context.
        /// </summary>
        public void Add(T entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        public void Update(T entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        public void Delete(int id)
        {
            var entity = dbSet.Find(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
