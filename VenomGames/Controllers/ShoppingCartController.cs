using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.CartItem;
using VenomGames.Core.DTOs.ShoppingCart;

namespace VenomGames.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // Display cart items
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _shoppingCartService.GetShoppingCartAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                ViewBag.Message = "Your shopping cart is empty.";
                return View(new ShoppingCartOutputModel());
            }

            var viewModel = new ShoppingCartOutputModel
            {
                TotalPrice = cart.TotalPrice,
                Items = cart.Items.Select(item => new CartItemOutputModel
                {
                    GameId = item.GameId,
                    Title = item.Title,
                    ImageUrl = item.ImageUrl,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user ID

            if (userId != null)
            {
                await _shoppingCartService.AddToCartAsync(userId, gameId, 1); // Add 1 quantity
                return RedirectToAction("Index", "ShoppingCart"); // Redirect to shopping cart page
            }

            // Redirect to login page if the user is not authenticated
            return RedirectToAction("Login", "Account");
        }


        // Update quantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _shoppingCartService.AddToCartAsync(userId, itemId, quantity);
            return RedirectToAction("Index");
        }

        // Remove item from cart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _shoppingCartService.RemoveFromCartAsync(userId, itemId);

            if (!result)
            {
                // Handle failure (optional)
                TempData["Error"] = "Failed to remove the item from the cart.";
            }

            return RedirectToAction("Index");
        }



        // Complete order
        [HttpPost]
        public async Task<IActionResult> CompleteOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _shoppingCartService.CompleteOrderAsync(userId);
            return RedirectToAction("OrderConfirmation", new { orderId = cart.Id });
        }

        // Order confirmation
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var cart = await _shoppingCartService.GetShoppingCartAsync(orderId);
            return View(cart);
        }
    }
}
