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
        public IEnumerable<Category> GetAllCategories()
        {
            return categoryRepository.GetAll();
        }

        /// <summary>
        /// Retrieves a specific category by ID.
        /// </summary>
        public Category GetCategoryById(int id)
        {
            return categoryRepository.GetById(id);
        }

        /// <summary>
        /// Adds a new category to the repository.
        /// </summary>
        public void CreateCategory(Category category)
        {
            categoryRepository.Add(category);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        public void UpdateCategory(Category category)
        {
            categoryRepository.Update(category);
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        public void DeleteCategory(int id)
        {
            categoryRepository.Delete(id);
        }
    }
}
