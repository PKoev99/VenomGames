﻿using VenomGames.Core.DTOs.Category;

namespace VenomGames.Core.Contracts
{
    /// <summary>
    /// Interface for Category service.
    /// Defines methods for category management.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>A single category.</returns>
        Task<CategoryOutputModel> GetCategoryDetailsAsync(int id);

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CategoryOutputModel>> GetAllCategoriesAsync();


        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="category">Category to be added.</param>
        Task CreateCategoryAsync(CategoryCreateDTO category);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">Category with updated information.</param>
        Task UpdateCategoryAsync(CategoryUpdateDTO category);

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">ID of the category to be deleted.</param>
        Task DeleteCategoryAsync(int id);
    }
}