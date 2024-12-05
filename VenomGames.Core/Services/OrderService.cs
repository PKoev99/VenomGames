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
        public async Task<IEnumerable<OrderOutputModel>> GetOrdersAsync(GetOrdersQuery query)
        {
            IQueryable<Order> orders = context.Orders;

            string? userId = query.UserId;
            if (!userId.IsNullOrEmpty())
            {
                orders = orders.Where(o => o.UserId==userId);
            }

            int? gameId = query.GameId;
            if (gameId.HasValue)
            {
                orders = orders.Where(o => o.GameOrders.Any(g=>g.Game.Id==gameId));
            }

            int? categoryId = query.CategoryId;
            if (categoryId.HasValue)
            {
                orders = orders.Where(o => o.GameOrders.Any(g => g.Order.Id==categoryId));
            }

            DateTime? startDate = query.StartDate;
            DateTime? endDate = query.EndDate;
            if (startDate.HasValue && endDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate >= startDate.Value.Date && o.OrderDate <= endDate.Value.Date);
            }

            IEnumerable<OrderOutputModel> ordersOutput = await orders
                .Select(g => new OrderOutputModel
                {
                    Id = g.Id,
                    OrderDate = g.OrderDate,
                    TotalPrice = g.TotalPrice,
                    UserId = g.UserId,
                    GameOrders = g.GameOrders
                }).ToListAsync();

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
                .Select(g => new OrderOutputModel
                {
                    Id = g.Id,
                    OrderDate = g.OrderDate,
                    TotalPrice = g.TotalPrice,
                    UserId = g.UserId,
                    GameOrders = g.GameOrders
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
                    Id= g.Id,
                    OrderDate = g.OrderDate,
                    TotalPrice = g.TotalPrice,
                    UserId = g.UserId,
                    GameOrders = g.GameOrders
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
