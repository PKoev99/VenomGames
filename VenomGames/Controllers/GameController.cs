using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Game;
using VenomGames.Core.Services;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService gameService;

        public GameController(IGameService _gameService)
        {
            gameService = _gameService;
        }

        // GET: /Games
        public async Task<IActionResult> Index()
        {
            IEnumerable<GameOutputModel> games = await gameService.GetGamesAsync(new GetGamesQuery());
            return View(games);
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Create(GameCreateDTO game)
        {
            if (ModelState.IsValid)
            {
                await gameService.CreateGameAsync(game);
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Game/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var game = await gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }


        // POST: Game/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Delete(int id)
        {
            var game = await gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game); 
        }


        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GameOutputModel game = await gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            await gameService.DeleteGameAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}