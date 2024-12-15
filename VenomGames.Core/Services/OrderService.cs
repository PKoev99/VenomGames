using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Order;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service class for managing orders.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;

        public OrderService(ApplicationDbContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// Searches for orders from the database.
        /// </summary>
        public async Task<IEnumerable<OrderOutputModel>> GetOrdersAsync()
        {
            IQueryable<Order> orders = context.Orders;

            IEnumerable<OrderOutputModel> ordersOutput = await orders
                .Select(o => new OrderOutputModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    UserId = o.UserId,
                    GameOrders = o.GameOrders.Select(go => new OrderItemDTO
                    {
                        GameName = go.Game.Title,
                        Price = go.Game.Price,
                        Quantity = go.Order.GameOrders.Count()
                    }).ToList()
                })
                .ToListAsync();

            return ordersOutput;

        }

        /// <summary>
        /// Get all orders for a specific user by his ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<OrderOutputModel>> GetOrdersByUserIdAsync(string userId)
        {
            IEnumerable<OrderOutputModel> ordersOutput = await context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderOutputModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    UserId = o.UserId,
                    GameOrders = o.GameOrders.Select(go => new OrderItemDTO
                    {
                        GameName = go.Game.Title,
                        Price = go.Game.Price,
                        Quantity = go.Order.GameOrders.Count() 
                    }).ToList()
                }).ToListAsync();

            return ordersOutput;

        }

        /// <summary>
        /// Retrieves details about a specific order by ID.
        /// </summary>
        public async Task<OrderOutputModel> GetOrderDetailsAsync(int id)
        {
            OrderOutputModel? order = await context.Orders
                .Where(g => g.Id == id)
                .Select(g => new OrderOutputModel
                {
                    Id = g.Id,
                    OrderDate = g.OrderDate,
                    TotalPrice = g.TotalPrice,
                    UserId = g.UserId,
                    GameOrders = g.GameOrders.Select(go => new OrderItemDTO
                    {
                        GameName = go.Game.Title,
                        Price = go.Game.Price,
                        Quantity = go.Quantity
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (order == null)
            {
                throw new NotFoundException(nameof(Order), id);
            }

            return order;
        }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        public async Task CreateOrderAsync(OrderCreateDTO order)
        {
            Order newOrder = new Order()
            {                
                OrderDate = order.OrderDate,
                TotalPrice = order.Price,
                UserId = order.UserId
            };

            context.Orders.Add(newOrder);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        public async Task UpdateOrderAsync(OrderUpdateDTO order)
        {
            Order newOrder = new Order()
            {                
                OrderDate = order.OrderDate,
                TotalPrice = order.Price,
                UserId = order.UserId
            };

            context.Orders.Update(newOrder);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        public async Task DeleteOrderAsync(int id)
        {
            Order? order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new NotFoundException(nameof(Order), id);
            }

            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }
    }
}