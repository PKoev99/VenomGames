using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Category;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service class for managing categories.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryOutputModel>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await context.Categories.ToListAsync();

            return categories.Select(c => new CategoryOutputModel
            {
                Id = c.CategoryId,
                Name = c.Name,
                Games = c.GameCategories
            });
        }

        /// <summary>
        /// Retrieves details about a specific category by ID.
        /// </summary>
        public async Task<CategoryOutputModel> GetCategoryDetailsAsync(int id)
        {
            Category? category = await context.Categories
                .Where(c => c.CategoryId == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), id);
            }

            CategoryOutputModel categoryOutput = new CategoryOutputModel
            {
                Id = category.CategoryId,
                Name = category.Name
            };

            return categoryOutput;
        }

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        public async Task CreateCategoryAsync(CategoryCreateDTO category)
        {
            Category newCategory = new Category()
            {
                Name = category.Name
            };

            context.Categories.Add(newCategory);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        public async Task UpdateCategoryAsync(CategoryUpdateDTO categoryDTO)
        {
            Category? category = await context.Categories
                .Where(c => c.CategoryId == categoryDTO.Id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), category.CategoryId);
            }

            category.Name = categoryDTO.Name;

            context.Categories.Update(category);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        public async Task DeleteCategoryAsync(int id)
        {
            Category? category = await context.Categories
                .Where(c=>c.CategoryId==id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), id);
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }
    }
}