using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using VenomGames.Controllers;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.CartItem;
using VenomGames.Core.DTOs.ShoppingCart;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Controllers
{
    [TestFixture]
    public class ShoppingCartControllerTests
    {
        private Mock<IShoppingCartService> mockCartService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private ShoppingCartController controller;

        private ClaimsPrincipal userPrincipal;

        [SetUp]
        public void Setup()
        {
            mockCartService = new Mock<IShoppingCartService>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            }, "mock"));

            controller = new ShoppingCartController(mockCartService.Object, mockUserManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = userPrincipal
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            controller.Dispose();
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithShoppingCart()
        {
            var mockCart = new ShoppingCartOutputModel
            {
                Items = new List<CartItemOutputModel>
                {
                    new CartItemOutputModel { GameId = 1, Name = "Test Game", Quantity = 2, Price = 50 }
                },
                TotalPrice = 100
            };

            mockCartService
                .Setup(s => s.GetShoppingCartAsync("test-user-id"))
                .ReturnsAsync(mockCart);

            var result = await controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as ShoppingCartOutputModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(100, model.TotalPrice);
            Assert.AreEqual(1, model.Items.Count);
        }

        [Test]
        public async Task Index_ReturnsEmptyCart_WhenNoItemsExist()
        {
            var emptyCart = new ShoppingCartOutputModel { Items = new List<CartItemOutputModel>(), TotalPrice = 0 };
            mockCartService
                .Setup(s => s.GetShoppingCartAsync("test-user-id"))
                .ReturnsAsync(emptyCart);

            var result = await controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as ShoppingCartOutputModel;
            Assert.AreEqual(0, model.Items.Count);
        }

        [Test]
        public async Task AddToCart_RedirectsToGameIndex_WhenUserIsLoggedIn()
        {
            var gameId = 1;

            mockCartService
                .Setup(s => s.AddToCartAsync("test-user-id", gameId, 1))
                .Returns(Task.CompletedTask);

            var result = await controller.AddToCart(gameId);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Game", redirectResult.ControllerName);
        }

        [Test]
        public async Task AddToCart_RedirectsToLogin_WhenUserIsNotLoggedIn()
        {
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            var result = await controller.AddToCart(1);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Login", redirectResult.ActionName);
            Assert.AreEqual("Account", redirectResult.ControllerName);
        }

        [Test]
        public async Task UpdateQuantity_RedirectsToIndex()
        {
            var itemId = 1;
            var quantity = 5;

            mockCartService
                .Setup(s => s.AddToCartAsync("test-user-id", itemId, quantity))
                .Returns(Task.CompletedTask);

            var result = await controller.UpdateQuantity(itemId, quantity);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }
        
        [Test]
        public async Task CompleteOrder_RedirectsToOrderConfirmation_WhenSuccess()
        {
            var cartOutput = new ShoppingCartOutputModel { Id = 1 };
            mockCartService
                .Setup(s => s.CompleteOrderAsync("test-user-id"))
                .ReturnsAsync(cartOutput);

            var result = await controller.CompleteOrder();

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("OrderConfirmation", redirectResult.ActionName);
            Assert.AreEqual(1, redirectResult.RouteValues["orderId"]);
        }
    }
}