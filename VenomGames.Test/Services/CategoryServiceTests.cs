using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.DTOs.Category;
using VenomGames.Core.Services;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Services
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options;
        private ApplicationDbContext context;
        private CategoryService categoryService;

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(options);
            categoryService = new CategoryService(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            context.Categories.AddRange(new List<Category>
            {
                new Category { CategoryId = 1, Name = "Category 1" },
                new Category { CategoryId = 2, Name = "Category 2" }
            });
            await context.SaveChangesAsync();

            var result = await categoryService.GetAllCategoriesAsync();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(c => c.Name == "Category 1"));
            Assert.IsTrue(result.Any(c => c.Name == "Category 2"));
        }

        [Test]
        public async Task GetCategoryDetailsAsync_ReturnsCategoryDetails_WhenCategoryExists()
        {
            var category = new Category { CategoryId = 1, Name = "Category 1" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var result = await categoryService.GetCategoryDetailsAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Category 1", result.Name);
        }

        [Test]
        public void GetCategoryDetailsAsync_ThrowsNotFoundException_WhenCategoryDoesNotExist()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await categoryService.GetCategoryDetailsAsync(1));
        }

        [Test]
        public async Task CreateCategoryAsync_AddsNewCategoryToDatabase()
        {
            var categoryDto = new CategoryCreateDTO { Name = "New Category" };

            await categoryService.CreateCategoryAsync(categoryDto);

            var createdCategory = await context.Categories.FirstOrDefaultAsync();
            Assert.IsNotNull(createdCategory);
            Assert.AreEqual("New Category", createdCategory.Name);
        }

        [Test]
        public async Task UpdateCategoryAsync_UpdatesCategoryInDatabase_WhenCategoryExists()
        {
            var category = new Category { CategoryId = 1, Name = "Old Name" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var updateDto = new CategoryUpdateDTO { Id = 1, Name = "Updated Name" };

            await categoryService.UpdateCategoryAsync(updateDto);

            var updatedCategory = await context.Categories.FindAsync(1);
            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual("Updated Name", updatedCategory.Name);
        }

        [Test]
        public void UpdateCategoryAsync_ThrowsNotFoundException_WhenCategoryDoesNotExist()
        {
            var updateDto = new CategoryUpdateDTO { Id = 1, Name = "Updated Name" };

            Assert.ThrowsAsync<NotFoundException>(async () => await categoryService.UpdateCategoryAsync(updateDto));
        }

        [Test]
        public async Task DeleteCategoryAsync_RemovesCategoryFromDatabase_WhenCategoryExists()
        {
            var category = new Category { CategoryId = 1, Name = "Category to Delete" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            await categoryService.DeleteCategoryAsync(1);

            var deletedCategory = await context.Categories.FindAsync(1);
            Assert.IsNull(deletedCategory);
        }

        [Test]
        public void DeleteCategoryAsync_ThrowsNotFoundException_WhenCategoryDoesNotExist()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await categoryService.DeleteCategoryAsync(1));
        }
    }
}
