using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.CartItem;
using VenomGames.Core.DTOs.ShoppingCart;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service for managing the shopping cart
    /// </summary>
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the shopping cart by user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the shopping cart by order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> GetShoppingCartAsync(int orderId)
        {
            return await _context.ShoppingCarts
                .Include(cart => cart.Items)
                .ThenInclude(item => item.Game)
                .Where(cart => cart.Id == orderId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds an item to the cart
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task AddToCartAsync(string userId, int gameId, int quantity)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCompleted);

            if (cart == null)
            {
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
                cartItem.Quantity += quantity;
            }

            cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates item quantity in cart.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task UpdateCartItemQuantityAsync(string userId, int itemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(itemId);

            if (cartItem != null && cartItem.ShoppingCart.UserId == userId)
            {
                cartItem.Quantity = quantity;
                cartItem.ShoppingCart.TotalPrice = (cartItem.Quantity * cartItem.Price);
                _context.Update(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes an item from the cart.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveFromCartAsync(string userId, int itemId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCompleted);

            if (cart == null)
            {
                return false;
            }

            var item = cart.Items.FirstOrDefault(i => i.GameId == itemId);

            if (item == null)
            {
                return false;
            }

            cart.Items.Remove(item);
            cart.TotalPrice -= item.Price * item.Quantity;

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Completes the cart into an order.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ShoppingCartOutputModel> CompleteOrderAsync(string userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCompleted);

            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Cannot complete an empty cart.");
            }

            var order = new Order
            {
                UserId = userId,
                TotalPrice = cart.TotalPrice,
                OrderDate = DateTime.UtcNow,
                GameOrders = cart.Items.Select(item => new GameOrder
                {
                    Quantity = item.Quantity,
                    GameId = item.GameId
                }).ToList()
            };

            _context.Orders.Add(order);

            cart.IsCompleted = true;
            cart.OrderDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ShoppingCartOutputModel
            {
                Id = cart.Id,
                TotalPrice = order.TotalPrice,
                IsCompleted = cart.IsCompleted,
                OrderDate = order.OrderDate ?? DateTime.UtcNow,
                Items = cart.Items.Select(i => new CartItemOutputModel
                {
                    GameId = i.GameId,
                    Title = i.Game.Title,
                    ImageUrl = i.Game.ImageUrl,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

        /// <summary>
        /// Get the amount of items in the cart.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetCartItemCountAsync(string userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCompleted);

            return cart?.Items.Sum(item => item.Quantity) ?? 0;
        }
    }
}