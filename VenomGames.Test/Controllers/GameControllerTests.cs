using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using VenomGames.Controllers;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Category;
using VenomGames.Core.DTOs.Game;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Controllers
{
    [TestFixture]
    public class GameControllerTests
    {
        private Mock<IGameService> mockGameService;
        private Mock<ICategoryService> mockCategoryService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<IShoppingCartService> mockCartService;
        private GameController controller;

        [SetUp]
        public void Setup()
        {
            mockGameService = new Mock<IGameService>();
            mockCategoryService = new Mock<ICategoryService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            mockCartService = new Mock<IShoppingCartService>();

            controller = new GameController(
                mockGameService.Object,
                mockCategoryService.Object,
                mockUserManager.Object,
                mockCartService.Object
            );

            controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, Mock.Of<ITempDataProvider>());
        }

        [TearDown]
        public void TearDown()
        {
            controller.Dispose();
        }           

        [Test]
        public async Task Details_WhenGameExists_ReturnsViewWithGame()
        {
            var gameId = 1;
            var gameDetails = new GameOutputModel { GameId = gameId, Title = "Game1", Description = "Description1" };

            mockGameService
                .Setup(service => service.GetGameDetailsAsync(gameId))
                .ReturnsAsync(gameDetails);

            var result = await controller.Details(gameId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as GameOutputModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(gameId, model.GameId);
        }

        [Test]
        public async Task Details_WhenGameDoesNotExist_ReturnsNotFound()
        {
            var gameId = 99;
            mockGameService
                .Setup(service => service.GetGameDetailsAsync(gameId))
                .ReturnsAsync((GameOutputModel)null);

            var result = await controller.Details(gameId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_GET_ReturnsViewWithCategories()
        {
            var categoryList = new List<CategoryOutputModel>
        {
            new CategoryOutputModel { Id = 1, Name = "Action" },
            new CategoryOutputModel { Id = 2, Name = "RPG" }
        };

            mockCategoryService
                .Setup(service => service.GetAllCategoriesAsync())
                .ReturnsAsync(categoryList);

            var result = await controller.Create();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsTrue(viewResult.ViewData.ContainsKey("Categories"));

            var categories = viewResult.ViewData["Categories"] as IEnumerable<SelectListItem>;
            Assert.IsNotNull(categories);
            Assert.AreEqual(2, categories.Count());
        }

        [Test]
        public async Task Create_POST_ValidGame_RedirectsToIndex()
        {
            var newGame = new GameCreateDTO
            {
                Title = "Test Game",
                Description = "Test Description",
                Price = 29.99M,
                ImageUrl = "image.jpg",
                SelectedCategoryIds = new List<int> { 1, 2 }
            };

            mockGameService
                .Setup(service => service.CreateGameAsync(newGame))
                .Returns(Task.CompletedTask);

            var result = await controller.Create(newGame);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task DeleteConfirmed_WhenGameExists_RedirectsToIndex()
        {
            var gameId = 1;
            mockGameService
                .Setup(service => service.DeleteGameAsync(gameId))
                .Returns(Task.CompletedTask);

            var result = await controller.DeleteConfirmed(gameId);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task DeleteConfirmed_WhenGameNotFound_ReturnsNotFound()
        {
            var gameId = 99;
            mockGameService
                .Setup(service => service.DeleteGameAsync(gameId))
                .ThrowsAsync(new NotFoundException());

            var result = await controller.DeleteConfirmed(gameId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
