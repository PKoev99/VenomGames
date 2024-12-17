using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VenomGames.Controllers;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Order;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Controllers
{
    [TestFixture]
    public class OrderControllerTests
    {
        private Mock<IOrderService> mockOrderService;
        private Mock<IShoppingCartService> mockCartService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private OrderController controller;

        [SetUp]
        public void Setup()
        {
            mockOrderService = new Mock<IOrderService>();

            mockCartService = new Mock<IShoppingCartService>();
            mockCartService
                .Setup(s => s.GetCartItemCountAsync(It.IsAny<string>()))
                .ReturnsAsync(0);

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            mockUserManager
                .Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .Returns("test-user-id");

            controller = new OrderController(mockOrderService.Object, mockCartService.Object, mockUserManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            controller.Dispose();
        }

        [Test]
        public async Task Details_ReturnsViewResult_WhenOrderExists()
        {
            var order = new OrderOutputModel
            {
                Id = 1,
                UserId = "test-user-id",
                TotalPrice = 99.99M,
                GameOrders = new List<OrderItemDTO>
                {
                    new OrderItemDTO { GameName = "Game1", Price = 50.0M, Quantity = 1 },
                    new OrderItemDTO { GameName = "Game2", Price = 49.99M, Quantity = 1 }
                }
            };

            mockOrderService
                .Setup(s => s.GetOrderDetailsAsync(1))
                .ReturnsAsync(order);

            var result = await controller.Details(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as OrderOutputModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            mockOrderService
                .Setup(s => s.GetOrderDetailsAsync(1))
                .ReturnsAsync((OrderOutputModel)null);

            var result = await controller.Details(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Create_Get_ReturnsViewResult()
        {
            var result = controller.Create();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            var orderDTO = new OrderCreateDTO { UserId = "test-user-id", Price = 100.0M };

            mockOrderService
                .Setup(s => s.CreateOrderAsync(orderDTO))
                .Returns(Task.CompletedTask);

            var result = await controller.Create(orderDTO);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            controller.ModelState.AddModelError("TotalPrice", "Required");

            var orderDTO = new OrderCreateDTO();

            var result = await controller.Create(orderDTO);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(orderDTO, viewResult.Model);
        }

        [Test]
        public async Task Edit_Get_ReturnsViewResult_WithOrder()
        {
            var order = new OrderOutputModel { Id = 1, TotalPrice = 99.99M };

            mockOrderService
                .Setup(s => s.GetOrderDetailsAsync(1))
                .ReturnsAsync(order);

            var result = await controller.Edit(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as OrderOutputModel;
            Assert.AreEqual(1, model.Id);
        }

        [Test]
        public async Task Edit_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            var orderDTO = new OrderUpdateDTO { Id = 1, Price = 150.0M };

            mockOrderService
                .Setup(s => s.UpdateOrderAsync(orderDTO))
                .Returns(Task.CompletedTask);

            var result = await controller.Edit(1, orderDTO);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Delete_Get_ReturnsViewResult_WithOrder()
        {
            var order = new OrderOutputModel { Id = 1, TotalPrice = 99.99M };

            mockOrderService
                .Setup(s => s.GetOrderDetailsAsync(1))
                .ReturnsAsync(order);

            var result = await controller.Delete(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as OrderOutputModel;
            Assert.AreEqual(1, model.Id);
        }

        [Test]
        public async Task DeleteConfirmed_Post_RedirectsToIndex()
        {
            mockOrderService
                .Setup(s => s.DeleteOrderAsync(1))
                .Returns(Task.CompletedTask);

            var result = await controller.DeleteConfirmed(1);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }
    }
}
