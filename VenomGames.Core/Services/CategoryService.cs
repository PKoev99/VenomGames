using Microsoft.EntityFrameworkCore;
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
        /// Searches for categories from the database.
        /// </summary>
        public async Task<IEnumerable<CategoryOutputModel>> GetCategoryAsync(GetCategoryQuery query)
        {
            IQueryable<Category> categories = context.Categories;

            string? categoryTitle = query.Name;
            if (!categoryTitle.IsNullOrEmpty())
            {
                categories = categories.Where(g => g.Name.Contains(categoryTitle!));
            }

            

            IEnumerable<CategoryOutputModel> categoryOutput = await categories
                .Select(c => new CategoryOutputModel
                {
                   Id = c.CategoryId,
                   Name = c.Name
                }).ToListAsync();

            return categoryOutput;

        }

        /// <summary>
        /// Retrieves details about a specific category by ID.
        /// </summary>
        public async Task<CategoryOutputModel> GetCategoryDetailsAsync(int id)
        {
            CategoryOutputModel? category = await context.Categories
                .Where(c => c.CategoryId == id)
                .Select(g => new CategoryOutputModel
                {
                   Id = g.CategoryId,
                   Name = g.Name
                }).FirstOrDefaultAsync();

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), id);
            }

            return category;
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
        public async Task UpdateCategoryAsync(CategoryUpdateDTO category)
        {
            Category newCategory = new Category()
            {
                Name = category.Name
            };

            context.Categories.Update(newCategory);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        public async Task DeleteCategoryAsync(int id)
        {
            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), id);
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
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
    }
}