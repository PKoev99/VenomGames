using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service class for managing orders.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> orderRepository;

        public OrderService(IRepository<Order> _orderRepository)
        {
            orderRepository = _orderRepository;
        }

        /// <summary>
        /// Retrieves all orders from the repository.
        /// </summary>
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await orderRepository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a specific order by ID.
        /// </summary>
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await orderRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Retrieves all orders by User ID.
        /// </summary>
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await orderRepository.GetAllAsync();
        }

        /// <summary>
        /// Adds a new order to the repository.
        /// </summary>
        public async Task CreateOrderAsync(Order order)
        {
            await orderRepository.AddAsync(order);
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        public async Task UpdateOrderAsync(Order order)
        {
            await orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        public async Task DeleteOrderAsync(int id)
        {
            await orderRepository.DeleteAsync(id);
        }
    }
}
