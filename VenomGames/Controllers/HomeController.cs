using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;
using VenomGames.Models.Home;

namespace VenomGames.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IGameService gameService;
        private readonly ICategoryService categoryService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IGameService _gameService, ICategoryService _categoryService, IShoppingCartService _shoppingCartService, UserManager<ApplicationUser> _userManager)
            :base(_shoppingCartService,_userManager)
        {
            gameService = _gameService;
            categoryService = _categoryService;
            userManager = _userManager;
            shoppingCartService = _shoppingCartService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            await SetCartItemCountAsync();

            var categories = categoryService.GetAllCategoriesAsync().Result;

            var games = categoryId.HasValue
                ? gameService.GetGamesByCategoryAsync(categoryId.Value).Result
                : gameService.GetFeaturedGamesAsync().Result;

            var selectedCategory = categories.FirstOrDefault(c => c.Id == categoryId);

            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);
                ViewBag.CartCount = await shoppingCartService.GetCartItemCountAsync(userId);
            }
            else
            {
                ViewBag.CartCount = 0;
            }

            var viewModel = new HomeViewModel
            {
                Categories = categories,
                FeaturedGames = games,
                CategoryName = selectedCategory?.Name
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
