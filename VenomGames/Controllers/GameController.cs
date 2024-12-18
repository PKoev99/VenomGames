using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Game;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    public class GameController : BaseController
    {
        private readonly IGameService gameService;
        private readonly ICategoryService categoryService;

        public GameController(IGameService _gameService, ICategoryService _categoryService, UserManager<ApplicationUser> _userManager, IShoppingCartService _shoppingCartService)
            :base(_shoppingCartService,_userManager)
        {
            gameService = _gameService;
            categoryService = _categoryService;
        }

        public async Task<IActionResult> Index(int page = 1, string searchQuery = "")
        {
            await SetCartItemCountAsync();

            const int pageSize = 10;

            var games = await gameService.GetAllGamesAsync();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                games = games.Where(game => game.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                             game.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var totalGames = games.Count();

            var totalPages = (int)Math.Ceiling(totalGames / (double)pageSize);

            var gamesOnCurrentPage = games.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new GameIndexOutputModel
            {
                Games = gamesOnCurrentPage,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchQuery = searchQuery
            };

            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            GameOutputModel? game = await gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(GameCreateDTO game)
        {
            if (ModelState.IsValid)
            {
                await gameService.CreateGameAsync(game);
                return RedirectToAction(nameof(Index));
            }

            var categories = await categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            return View(game);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var categories = await categoryService.GetAllCategoriesAsync();

            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = game.SelectedCategoryIds.Contains(c.Id)
            });

            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, GameUpdateDTO gameUpdateDTO)
        {
            if (id != gameUpdateDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var game = await gameService.GetGameDetailsAsync(id);
                if (game == null)
                {
                    return NotFound();
                }

                game.Title = gameUpdateDTO.Title;
                game.Price = gameUpdateDTO.Price;
                game.Description = gameUpdateDTO.Description;
                game.ImageUrl = gameUpdateDTO.ImageUrl;

                await gameService.UpdateGameAsync(game);

                return RedirectToAction(nameof(Details), new { id = game.GameId });
            }

            return View(gameUpdateDTO);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var game = await gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game); 
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed([FromForm] int id)
        {
            try
            {
                await gameService.DeleteGameAsync(id);
                return RedirectToAction("Index");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View();
            }
        }
    }
}