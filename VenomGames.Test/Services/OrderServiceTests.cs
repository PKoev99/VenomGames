using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.DTOs.Order;
using VenomGames.Core.Services;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private ApplicationDbContext context;
        private OrderService orderService;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(options);
            orderService = new OrderService(context);

            var game = new Game { Id = 1, Title = "Test Game", Price = 50m };
            var user = new ApplicationUser { Id = "user1" };

            var order = new Order
            {
                Id = 1,
                UserId = "user1",
                OrderDate = DateTime.UtcNow,
                TotalPrice = 100m,
                GameOrders = new List<GameOrder>
                {
                    new GameOrder { GameId = 1, Game = game, Quantity = 2 }
                }
            };

            await context.Games.AddAsync(game);
            await context.Users.AddAsync(user);
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task GetOrdersAsync_ReturnsAllOrders()
        {
            var orders = await orderService.GetOrdersAsync();

            Assert.AreEqual(1, orders.Count());
            Assert.AreEqual(1, orders.First().Id);
        }

        [Test]
        public async Task GetOrdersByUserIdAsync_ReturnsOrdersForUser()
        {
            var orders = await orderService.GetOrdersByUserIdAsync("user1");

            Assert.AreEqual(1, orders.Count());
            Assert.AreEqual("user1", orders.First().UserId);
        }

        [Test]
        public async Task GetOrdersByUserIdAsync_UserNotFound_ReturnsEmpty()
        {
            var orders = await orderService.GetOrdersByUserIdAsync("invalid_user");

            Assert.IsEmpty(orders);
        }

        [Test]
        public async Task GetOrderDetailsAsync_ValidId_ReturnsOrderDetails()
        {
            var order = await orderService.GetOrderDetailsAsync(1);

            Assert.IsNotNull(order);
            Assert.AreEqual(1, order.Id);
            Assert.AreEqual(100m, order.TotalPrice);
        }

        [Test]
        public void GetOrderDetailsAsync_InvalidId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await orderService.GetOrderDetailsAsync(999));
        }

        [Test]
        public async Task CreateOrderAsync_AddsNewOrder()
        {
            var newOrder = new OrderCreateDTO
            {
                UserId = "user1",
                OrderDate = DateTime.UtcNow,
                Price = 200m
            };

            await orderService.CreateOrderAsync(newOrder);

            Assert.AreEqual(2, context.Orders.Count());
            Assert.AreEqual(200m, context.Orders.Last().TotalPrice);
        }

        [Test]
        public async Task UpdateOrderAsync_ValidOrder_UpdatesOrder()
        {
            var updatedOrder = new OrderUpdateDTO
            {
                Id = 1,
                UserId = "user1",
                OrderDate = DateTime.UtcNow,
                Price = 150m
            };

            await orderService.UpdateOrderAsync(updatedOrder);

            var order = await context.Orders.FindAsync(1);
            Assert.AreEqual(150m, order.TotalPrice);
        }

        [Test]
        public void UpdateOrderAsync_InvalidId_ThrowsNotFoundException()
        {
            var updatedOrder = new OrderUpdateDTO
            {
                Id = 999,
                UserId = "user1",
                OrderDate = DateTime.UtcNow,
                Price = 150m
            };

            Assert.ThrowsAsync<NotFoundException>(async () => await orderService.UpdateOrderAsync(updatedOrder));
        }

        [Test]
        public async Task DeleteOrderAsync_ValidId_DeletesOrder()
        {
            await orderService.DeleteOrderAsync(1);

            Assert.AreEqual(0, context.Orders.Count());
        }

        [Test]
        public void DeleteOrderAsync_InvalidId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await orderService.DeleteOrderAsync(999));
        }
    }
}
