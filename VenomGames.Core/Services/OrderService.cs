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
        public IEnumerable<Order> GetAllOrders()
        {
            return orderRepository.GetAll();
        }

        /// <summary>
        /// Retrieves a specific order by ID.
        /// </summary>
        public Order GetOrderById(int id)
        {
            return orderRepository.GetById(id);
        }

        /// <summary>
        /// Retrieves all orders by User ID.
        /// </summary>
        public IEnumerable<Order> GetOrdersByUserId(string userId)
        {
            return orderRepository.GetAll().Where(o => o.UserId == userId);
        }

        /// <summary>
        /// Adds a new order to the repository.
        /// </summary>
        public void CreateOrder(Order order)
        {
            orderRepository.Add(order);
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        public void UpdateOrder(Order order)
        {
            orderRepository.Update(order);
        }

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        public void DeleteOrder(int id)
        {
            orderRepository.Delete(id);
        }
    }
}
