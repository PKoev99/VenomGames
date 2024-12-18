using VenomGames.Core.DTOs.ShoppingCart;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Contracts
{
    /// <summary>
    /// Interface for UserCart service.
    /// Defines methods for user cart management.
    /// </summary>
    public interface IShoppingCartService
    {
        /// <summary>
        /// Retrieves the users cart by user id
        /// </summary>
        /// <returns></returns>
        Task<ShoppingCartOutputModel> GetShoppingCartAsync(string userId);

        /// <summary>
        /// Retrieves the users cart by order id
        /// </summary>
        /// <returns></returns>
        Task<ShoppingCart> GetShoppingCartAsync(int orderId);
        
        /// <summary>
        /// Updates the item quantity in the cart
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task UpdateCartItemQuantityAsync(string userId, int itemId, int quantity);

        /// <summary>
        /// Adds a new item to the cart
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        Task AddToCartAsync(string userId, int gameId, int quantity);

        /// <summary>
        /// Removes an item from the cart
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task<bool> RemoveFromCartAsync(string userId, int itemId);

        /// <summary>
        /// Completes the cart order
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ShoppingCartOutputModel> CompleteOrderAsync(string userId);

        /// <summary>
        /// Get amount of items in the cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetCartItemCountAsync(string userId);
    }
}