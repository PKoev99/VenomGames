using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service class for managing categories.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> categoryRepository;

        public CategoryService(IRepository<Category> _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }

        /// <summary>
        /// Retrieves all categories from the repository.
        /// </summary>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await categoryRepository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a specific category by ID.
        /// </summary>
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await categoryRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Adds a new category to the repository.
        /// </summary>
        public async Task CreateCategoryAsync(Category category)
        {
            await categoryRepository.AddAsync(category);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        public async Task UpdateCategoryAsync (Category category)
        {
            await categoryRepository.UpdateAsync(category);
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        public async Task DeleteCategoryAsync(int id)
        {
            await categoryRepository.DeleteAsync(id);
        }
    }
}
