using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Services;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Services
{
    [TestFixture]
    public class ShoppingCartServiceTests
    {
        private ShoppingCartService shoppingCartService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            dbContext = new ApplicationDbContext(options);

            SeedDatabase();

            shoppingCartService = new ShoppingCartService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        private void SeedDatabase()
        {
            dbContext.Games.AddRange(new Game
            {
                Id = 1,
                Title = "Game 1",
                Price = 10.99m,
                ImageUrl = "image1.jpg"
            },
            new Game
            {
                Id = 2,
                Title = "Game 2",
                Price = 15.49m,
                ImageUrl = "image2.jpg"
            });

            dbContext.SaveChanges();
        }

        [Test]
        public async Task GetShoppingCartAsync_ShouldReturnEmptyCart_WhenNoCartExistsForUser()
        {
            var result = await shoppingCartService.GetShoppingCartAsync("user1");

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count);
            Assert.AreEqual(0, result.TotalPrice);
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddItemToCart()
        {
            await shoppingCartService.AddToCartAsync("user1", 1, 2);

            var cart = await dbContext.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(c => c.UserId == "user1");

            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.Items.Count);
            Assert.AreEqual(21.98m, cart.TotalPrice);
        }

        [Test]
        public async Task UpdateCartItemQuantityAsync_ShouldUpdateItemQuantity()
        {
            await shoppingCartService.AddToCartAsync("user1", 1, 2);
            var cartItem = dbContext.CartItems.First();

            await shoppingCartService.UpdateCartItemQuantityAsync("user1", cartItem.Id, 5);

            Assert.AreEqual(5, cartItem.Quantity);
            Assert.AreEqual(54.95m, cartItem.ShoppingCart.TotalPrice);
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldRemoveItemFromCart()
        {
            await shoppingCartService.AddToCartAsync("user1", 1, 2);
            var cart = await dbContext.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == "user1");
            var cartItem = cart.Items.First();

            var result = await shoppingCartService.RemoveFromCartAsync("user1", cartItem.GameId);

            cart = await dbContext.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == "user1");

            Assert.IsTrue(result);
            Assert.IsNotNull(cart);
            Assert.AreEqual(0, cart.Items.Count);
            Assert.AreEqual(0, cart.TotalPrice);
        }

        [Test]
        public async Task CompleteOrderAsync_ShouldCompleteTheCart()
        {
            await shoppingCartService.AddToCartAsync("user1", 1, 2);

            var result = await shoppingCartService.CompleteOrderAsync("user1");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsCompleted);
            Assert.AreEqual(21.98m, result.TotalPrice);

            var order = dbContext.Orders.FirstOrDefault(o => o.UserId == "user1");

            Assert.IsNotNull(order);
            Assert.AreEqual(21.98m, order.TotalPrice);
        }

        [Test]
        public async Task GetCartItemCountAsync_ShouldReturnCorrectItemCount()
        {
            await shoppingCartService.AddToCartAsync("user1", 1, 2);
            await shoppingCartService.AddToCartAsync("user1", 2, 3);

            var itemCount = await shoppingCartService.GetCartItemCountAsync("user1");

            Assert.AreEqual(5, itemCount);
        }
    }

}
