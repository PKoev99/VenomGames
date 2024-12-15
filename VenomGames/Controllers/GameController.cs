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

        // GET: /Games
        public async Task<IActionResult> Index(int page = 1, string searchQuery = "")
        {
            await SetCartItemCountAsync();

            // Define how many games per page
            const int pageSize = 10;

            // Get all games
            var games = await gameService.GetAllGamesAsync();

            // Apply search query filter if provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                games = games.Where(game => game.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                             game.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Get the total number of games
            var totalGames = games.Count();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalGames / (double)pageSize);

            // Get the games for the current page
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

        // GET: /Games/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GameOutputModel? game = await gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // GET: /Games/Create
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

        // POST: /Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(GameCreateDTO game)
        {
            if (ModelState.IsValid)
            {
                await gameService.CreateGameAsync(game); // Pass the DTO to your service for processing
                return RedirectToAction(nameof(Index));
            }

            // Reload categories if model validation fails
            var categories = await categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            return View(game);
        }

        // GET: Game/Edit/{id}
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var categories = await categoryService.GetAllCategoriesAsync();

            // Passing the current categories of the game to the view
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                // Check if the category is selected for the game
                Selected = game.SelectedCategoryIds.Contains(c.Id)
            });

            return View(game);
        }



        // POST: Game/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, GameUpdateDTO gameUpdateDTO)
        {
            if (id != gameUpdateDTO.Id)  // Ensure the ID in the URL matches the one from the form
            {
                return NotFound();  // If IDs don't match, return 404
            }

            if (ModelState.IsValid)
            {
                // Get the existing game from the database
                var game = await gameService.GetGameDetailsAsync(id);
                if (game == null)
                {
                    return NotFound();  // If the game doesn't exist, return 404
                }

                // Update the game properties with the new values from the form
                game.Title = gameUpdateDTO.Title;
                game.Price = gameUpdateDTO.Price;
                game.Description = gameUpdateDTO.Description;
                game.ImageUrl = gameUpdateDTO.ImageUrl;

                // Update the game in the database
                await gameService.UpdateGameAsync(game);

                // Redirect to the game's details page or the index page after successful update
                return RedirectToAction(nameof(Details), new { id = game.GameId });
            }

            // If model validation fails, return to the edit view with the current game data
            return View(gameUpdateDTO);
        }



        // GET: /Games/Delete/5
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
                // Await the DeleteGameAsync method to ensure it completes before redirecting
                await gameService.DeleteGameAsync(id);
                return RedirectToAction("Index"); // Redirect to the index view after deletion
            }
            catch (NotFoundException)
            {
                // If the game is not found, return a NotFound error
                return NotFound();
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(); // Optionally, return an error view
            }
        }

    }
}