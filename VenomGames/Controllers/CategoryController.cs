using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Category;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService _categoryService, IShoppingCartService _shoppingCartService, UserManager<ApplicationUser> _userManager)
            :base(_shoppingCartService,_userManager)
        {
            categoryService = _categoryService;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            await SetCartItemCountAsync();

            var categories = await categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // GET: /Category/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO category)
        {
            if (ModelState.IsValid)
            {
                await categoryService.CreateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/{id}
        public async Task<IActionResult> Edit(int id)
       {
            var category = await categoryService.GetCategoryDetailsAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryUpdateDTO category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await categoryService.UpdateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: /Category/Details/5
        public async Task<IActionResult> Details(int id)
        {
            CategoryOutputModel? category = await categoryService.GetCategoryDetailsAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: Category/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryService.GetCategoryDetailsAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await categoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
