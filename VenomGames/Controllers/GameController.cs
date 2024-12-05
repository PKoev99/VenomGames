using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Game;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // GET: /Games
        public async Task<IActionResult> Index(GetGamesQuery query)
        {
            var games = await _gameService.GetGamesAsync(query);
            return View(games);
        }

        // GET: /Games/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameService.GetGameDetailsAsync(id);
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
                await _gameService.CreateGameAsync(game);
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: /Games/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: /Games/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GameUpdateDTO game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _gameService.UpdateGameAsync(game);
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: /Games/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _gameService.GetGameDetailsAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: /Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gameService.DeleteGameAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
