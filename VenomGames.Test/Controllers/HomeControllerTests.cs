using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VenomGames.Controllers;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Category;
using VenomGames.Core.DTOs.Game;
using VenomGames.Infrastructure.Data.Models;
using VenomGames.Models;
using VenomGames.Models.Home;

namespace VenomGames.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IGameService> mockGameService;
        private Mock<ICategoryService> mockCategoryService;
        private Mock<IShoppingCartService> mockShoppingCartService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private HomeController controller;

        [SetUp]
        public void SetUp()
        {
            mockGameService = new Mock<IGameService>();
            mockCategoryService = new Mock<ICategoryService>();
            mockShoppingCartService = new Mock<IShoppingCartService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            controller = new HomeController(
                mockGameService.Object,
                mockCategoryService.Object,
                mockShoppingCartService.Object,
                mockUserManager.Object
            );

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [TearDown] public void TearDown()
        { 
            controller.Dispose();
        }


        [Test]
        public async Task Index_WhenCategoryIdIsNull_ShouldReturnFeaturedGamesAndCategories()
        {
            var categories = new List<CategoryOutputModel>
            {
                new CategoryOutputModel { Id = 1, Name = "Action" },
                new CategoryOutputModel { Id = 2, Name = "Adventure" }
            };

            var featuredGames = new List<GameOutputModel>
            {
                new GameOutputModel { GameId = 1, Title = "Game1" },
                new GameOutputModel { GameId = 2, Title = "Game2" }
            };

            mockCategoryService.Setup(s => s.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            mockGameService.Setup(s => s.GetFeaturedGamesAsync())
                .ReturnsAsync(featuredGames);

            var result = await controller.Index(null) as ViewResult;

            Assert.IsNotNull(result);
            var viewModel = result.Model as HomeViewModel;
            Assert.IsNotNull(viewModel);

            Assert.AreEqual(2, viewModel.Categories.Count());
            Assert.AreEqual(2, viewModel.FeaturedGames.Count());
            Assert.IsNull(viewModel.CategoryName);
        }

        [Test]
        public async Task Index_WhenCategoryIdIsProvided_ShouldReturnFilteredGamesAndCategories()
        {
            int categoryId = 1;

            var categories = new List<CategoryOutputModel>
            {
                new CategoryOutputModel { Id = 1, Name = "Action" },
                new CategoryOutputModel { Id = 2, Name = "Adventure" }
            };

            var gamesByCategory = new List<GameOutputModel>
            {
                new GameOutputModel { GameId = 1, Title = "FilteredGame1" }
            };

            mockCategoryService.Setup(s => s.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            mockGameService.Setup(s => s.GetGamesByCategoryAsync(categoryId))
                .ReturnsAsync(gamesByCategory);

            var result = await controller.Index(categoryId) as ViewResult;

            Assert.IsNotNull(result);
            var viewModel = result.Model as HomeViewModel;
            Assert.IsNotNull(viewModel);

            Assert.AreEqual(2, viewModel.Categories.Count());
            Assert.AreEqual(1, viewModel.FeaturedGames.Count());
            Assert.AreEqual("Action", viewModel.CategoryName);
        }

        [Test]
        public void Error_WhenStatusCodeIs404_ShouldReturnNotFoundView()
        {
            int statusCode = 404;

            var result = controller.Error(statusCode) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("404NotFound", result.ViewName);
        }

        [Test]
        public void Error_WhenStatusCodeIs500_ShouldReturnServerErrorView()
        {
            int statusCode = 500;

            var result = controller.Error(statusCode) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("500ServerError", result.ViewName);
        }

        [Test]
        public void Error_WhenStatusCodeIsOther_ShouldReturnGenericErrorView()
        {
            int statusCode = 400;

            var result = controller.Error(statusCode) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
            Assert.IsInstanceOf<ErrorViewModel>(result.Model);
        }
    }
}
