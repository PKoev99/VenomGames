using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using VenomGames.Controllers;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Review;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Controllers
{
    [TestFixture]
    public class ReviewControllerTests
    {
        private Mock<IReviewService> mockReviewService;
        private Mock<IShoppingCartService> mockCartService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private ReviewController controller;

        [SetUp]
        public void Setup()
        {
            mockReviewService = new Mock<IReviewService>();

            mockCartService = new Mock<IShoppingCartService>();
            mockCartService
                .Setup(s => s.GetCartItemCountAsync(It.IsAny<string>()))
                .ReturnsAsync(0);

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            mockUserManager
                .Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("test-user-id");

            controller = new ReviewController(mockReviewService.Object, mockCartService.Object, mockUserManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            controller.Dispose();
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithReview()
        {
            var review = new ReviewOutputModel { ReviewId = 1, Content = "Fantastic!", Rating = 5 };
            mockReviewService
                .Setup(s => s.GetReviewDetailsAsync(1))
                .ReturnsAsync(review);

            var result = await controller.Details(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as ReviewOutputModel;
            Assert.AreEqual(review.ReviewId, model.ReviewId);
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenReviewDoesNotExist()
        {
            mockReviewService
                .Setup(s => s.GetReviewDetailsAsync(1))
                .ReturnsAsync((ReviewOutputModel)null);

            var result = await controller.Details(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Create_Get_ReturnsViewResult_WithGameId()
        {
            var result = controller.Create(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as ReviewCreateDTO;
            Assert.AreEqual(1, model.GameId);
        }

        [Test]
        public async Task Create_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            var reviewDTO = new ReviewCreateDTO { GameId = 1, Content = "Awesome game!", Rating = 4 };
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            }));

            mockReviewService
                .Setup(s => s.CreateReviewAsync(reviewDTO, "test-user-id"))
                .Returns(Task.CompletedTask);

            var result = await controller.Create(reviewDTO);

            var redirect = result as RedirectToActionResult;
            Assert.IsNotNull(redirect);
            Assert.AreEqual("Index", redirect.ActionName);
            Assert.AreEqual(1, redirect.RouteValues["id"]);
        }

        [Test]
        public async Task Edit_Get_ReturnsViewResult_WhenReviewExists()
        {
            var review = new ReviewOutputModel { ReviewId = 1, Content = "Updated review" };
            mockReviewService
                .Setup(s => s.GetReviewDetailsAsync(1))
                .ReturnsAsync(review);

            var result = await controller.Edit(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as ReviewOutputModel;
            Assert.AreEqual(1, model.ReviewId);
        }

        [Test]
        public async Task Edit_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            var reviewDTO = new ReviewUpdateDTO { Id = 1, GameId = 1, Content = "Updated review" };
            mockReviewService
                .Setup(s => s.UpdateReviewAsync(reviewDTO))
                .Returns(Task.CompletedTask);

            var result = await controller.Edit(1, reviewDTO);

            var redirect = result as RedirectToActionResult;
            Assert.IsNotNull(redirect);
            Assert.AreEqual("Index", redirect.ActionName);
            Assert.AreEqual(1, redirect.RouteValues["gameId"]);
        }

        [Test]
        public async Task DeleteConfirmed_RedirectsToIndex_AfterDeletion()
        {
            var review = new ReviewOutputModel { ReviewId = 1, GameId = 1 };
            mockReviewService
                .Setup(s => s.GetReviewDetailsAsync(1))
                .ReturnsAsync(review);

            mockReviewService
                .Setup(s => s.DeleteReviewAsync(1))
                .Returns(Task.CompletedTask);

            var result = await controller.DeleteConfirmed(1);

            var redirect = result as RedirectToActionResult;
            Assert.IsNotNull(redirect);
            Assert.AreEqual("Index", redirect.ActionName);
            Assert.AreEqual(1, redirect.RouteValues["gameId"]);
        }
    }
}
