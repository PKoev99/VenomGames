﻿using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Contracts
{
    /// <summary>
    /// Interface for Order service.
    /// Defines methods for order management.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Retrieves all orders placed by users.
        /// </summary>
        /// <returns>List of all orders.</returns>
        IEnumerable<Order> GetAllOrders();

        /// <summary>
        /// Retrieves all orders placed by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>List of orders placed by the user.</returns>
        IEnumerable<Order> GetOrdersByUserId(int userId);

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>A single order.</returns>
        Order GetOrderById(int id);

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        /// <param name="order">Order to be added.</param>
        void CreateOrder(Order order);

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="order">Order with updated information.</param>
        void UpdateOrder(Order order);

        /// <summary>
        /// Deletes an order by its ID.
        /// </summary>
        /// <param name="id">ID of the order to be deleted.</param>
        void DeleteOrder(int id);
    }
}