using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    public class BaseController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly UserManager<ApplicationUser> userManager;

        protected BaseController(IShoppingCartService _shoppingCartService, UserManager<ApplicationUser> _userManager)
        {
            shoppingCartService = _shoppingCartService;
            userManager = _userManager;
        }

        protected async Task SetCartItemCountAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);
                ViewBag.CartCount = await shoppingCartService.GetCartItemCountAsync(userId);
            }
            else
            {
                ViewBag.CartCount = 0;
            }
        }
    }
}
