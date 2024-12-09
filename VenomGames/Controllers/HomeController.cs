using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Models.Category;
using VenomGames.Models.Game;
using VenomGames.Models.Home;

namespace VenomGames.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;

        public HomeController(IGameService gameService, ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var featuredGames = (await _gameService.GetFeaturedGamesAsync()).Select(game => new GameViewModel
            {
                Id = game.GameId,
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Price = game.Price
            });

            var categories = (await _categoryService.GetAllCategoriesAsync()).Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            });

            var model = new HomeViewModel
            {
                FeaturedGames = featuredGames,
                Categories = categories
            };

            return View(model);
        }

        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
