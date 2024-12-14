﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.ShoppingCart;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(IShoppingCartService _shoppingCartService, UserManager<ApplicationUser> _userManager)
            :base(_shoppingCartService,_userManager)
        {
            shoppingCartService = _shoppingCartService;
        }

        // Display cart items
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await SetCartItemCountAsync();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var shoppingCart = await shoppingCartService.GetShoppingCartAsync(userId);

            if (shoppingCart == null || !shoppingCart.Items.Any())
            {
                return View(new ShoppingCartOutputModel());
            }

            return View(shoppingCart);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user ID

            if (userId != null)
            {
                await shoppingCartService.AddToCartAsync(userId, gameId, 1); // Add 1 quantity
                return RedirectToAction("Index", "Game");
            }

            // Redirect to login page if the user is not authenticated
            return RedirectToAction("Login", "Account");
        }


        // Update quantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await shoppingCartService.AddToCartAsync(userId, itemId, quantity);
            return RedirectToAction("Index");
        }

        // Remove item from cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            Console.WriteLine("Received Item ID: " + itemId);
            if (itemId == 0)
            {
                // Handle the case where the ID is not valid
                TempData["ErrorMessage"] = "Invalid item ID.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await shoppingCartService.RemoveFromCartAsync(userId, itemId);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to remove the item from the cart.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Item removed successfully!";
            return RedirectToAction("Index");
        }

        // Complete order
        [HttpPost]
        public async Task<IActionResult> CompleteOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cart = await shoppingCartService.CompleteOrderAsync(userId);
                return RedirectToAction("OrderConfirmation", new { orderId = cart.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Order confirmation
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var cart = await shoppingCartService.GetShoppingCartAsync(orderId);
            return View(cart);
        }
    }
}
