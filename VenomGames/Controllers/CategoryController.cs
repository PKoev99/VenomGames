using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Category;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService _categoryService)
        {
            categoryService = _categoryService;
        }

        // GET: /Category
        public async Task<IActionResult> Index(GetCategoryQuery query)
        {
            IEnumerable<CategoryOutputModel> categories = await categoryService.GetCategoryAsync(query);
            return View(categories);
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

        // GET: /Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
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

        // GET: /Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            CategoryOutputModel? category = await categoryService.GetCategoryDetailsAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: /Category/Edit/5
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

        // GET: /Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            CategoryOutputModel? category = await categoryService.GetCategoryDetailsAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await categoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
