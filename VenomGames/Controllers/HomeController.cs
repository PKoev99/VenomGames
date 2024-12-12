using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Game;
using VenomGames.Models.Home;

namespace VenomGames.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService gameService;
        private readonly ICategoryService categoryService;
        private readonly IShoppingCartService userCartService;

        public HomeController(IGameService _gameService, ICategoryService _categoryService, IShoppingCartService _userCartService)
        {
            gameService = _gameService;
            categoryService = _categoryService;
            userCartService = _userCartService;
        }

        public async Task<IActionResult> Index(int categoryId)
        {     

            var categories = await categoryService.GetAllCategoriesAsync();

            IEnumerable<GameOutputModel> games = categoryId == null ? await gameService.GetFeaturedGamesAsync() : await gameService.GetGamesByCategoryAsync(categoryId);

            var viewModel = new HomeViewModel
            {
                Categories = await categoryService.GetAllCategoriesAsync(),
                FeaturedGames = await gameService.GetFeaturedGamesAsync()
            };

            return View(viewModel);
        }

        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
