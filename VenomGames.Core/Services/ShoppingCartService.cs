using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;
using VenomGames.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VenomGames.Core.DTOs.ShoppingCart;
using VenomGames.Core.DTOs.CartItem;

namespace VenomGames.Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get shopping cart by user ID
        public async Task<ShoppingCartOutputModel> GetShoppingCartAsync(string userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCompleted);

            if (cart == null)
            {
                return new ShoppingCartOutputModel();
            }

            return new ShoppingCartOutputModel
            {
                TotalPrice = cart.TotalPrice,
                Items = cart.Items.Select(i => new CartItemOutputModel
                {
                    GameId = i.Game.Id,
                    Title = i.Game.Title,
                    ImageUrl = i.Game.ImageUrl,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

        }

        // Get shopping cart by order ID (for confirmation)
        public async Task<ShoppingCart> GetShoppingCartAsync(int orderId)
        {
            return await _context.ShoppingCarts
                .Include(cart => cart.Items)
                .ThenInclude(item => item.Game)
                .Where(cart => cart.Id == orderId)
                .FirstOrDefaultAsync();
        }

        // Add an item to the shopping cart
        public async Task AddToCartAsync(string userId, int gameId, int quantity)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCompleted);

            if (cart == null)
            {
                // Create a new cart if none exists
                cart = new ShoppingCart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    IsCompleted = false,
                    Items = new List<CartItem>()
                };
                _context.ShoppingCarts.Add(cart);
            }

            var cartItem = cart.Items.FirstOrDefault(ci => ci.GameId == gameId);
            if (cartItem == null)
            {
                // Add new item to the cart
                cartItem = new CartItem
                {
                    GameId = gameId,
                    ShoppingCart = cart,
                    Quantity = quantity,
                    Price = (await _context.Games.FindAsync(gameId))?.Price ?? 0
                };
                cart.Items.Add(cartItem);
            }
            else
            {
                // Update quantity if the game is already in the cart
                cartItem.Quantity += quantity;
            }

            cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity); // Update total price
            await _context.SaveChangesAsync(); // Persist changes
        }


        // Update the quantity of an item in the cart
        public async Task UpdateCartItemQuantityAsync(string userId, int itemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(itemId);

            if (cartItem != null && cartItem.ShoppingCart.UserId == userId)
            {
                cartItem.Quantity = quantity;
                _context.Update(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        // Remove an item from the shopping cart
        public async Task<bool> RemoveFromCartAsync(string userId, int itemId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCompleted);

            if (cart == null)
            {
                return false;
            }

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                return false;
            }

            cart.Items.Remove(item);
            cart.TotalPrice -= item.Price * item.Quantity;

            await _context.SaveChangesAsync();

            return true;
        }


        // Complete the order and mark the cart as completed
        public async Task<ShoppingCartOutputModel> CompleteOrderAsync(string userId)
        {
            var cart = await GetShoppingCartAsync(userId);

            if (cart.Items.Count == 0)
            {
                throw new InvalidOperationException("Cannot complete an empty cart.");
            }

            cart.IsCompleted = true;
            cart.OrderDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Optionally, you can also create an order in an Orders table here
            return cart;
        }
    }
}
