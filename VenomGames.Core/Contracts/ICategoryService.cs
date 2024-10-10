using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Contracts
{
    /// <summary>
    /// Interface for Category service.
    /// Defines methods for category management.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>List of all categories.</returns>
        IEnumerable<Category> GetAllCategories();

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>A single category.</returns>
        Category GetCategoryById(int id);

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="category">Category to be added.</param>
        void CreateCategory(Category category);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">Category with updated information.</param>
        void UpdateCategory(Category category);

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">ID of the category to be deleted.</param>
        void DeleteCategory(int id);
    }
}
