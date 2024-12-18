using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VenomGames.Controllers;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Category;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Controllers
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> mockCategoryService;
        private Mock<IShoppingCartService> mockCartService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private CategoryController controller;

        [SetUp]
        public void Setup()
        {
            mockCategoryService = new Mock<ICategoryService>();

            mockCartService = new Mock<IShoppingCartService>();
            mockCartService
                .Setup(s => s.GetCartItemCountAsync(It.IsAny<string>()))
                .ReturnsAsync(0);

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            controller = new CategoryController(mockCategoryService.Object, mockCartService.Object, mockUserManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            controller.Dispose();
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
            var categoryDTO = new CategoryCreateDTO { Name = "Adventure" };

            mockCategoryService
                .Setup(s => s.CreateCategoryAsync(categoryDTO))
                .Returns(Task.CompletedTask);

            var result = await controller.Create(categoryDTO);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            controller.ModelState.AddModelError("Name", "Required");

            var categoryDTO = new CategoryCreateDTO();

            var result = await controller.Create(categoryDTO);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(categoryDTO, viewResult.Model);
        }

        [Test]
        public async Task Edit_Get_ReturnsViewResult_WithCategory()
        {
            var category = new CategoryOutputModel { Id = 1, Name = "Adventure" };

            mockCategoryService
                .Setup(s => s.GetCategoryDetailsAsync(1))
                .ReturnsAsync(category);

            var result = await controller.Edit(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as CategoryOutputModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [Test]
        public async Task Edit_Get_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            mockCategoryService
                .Setup(s => s.GetCategoryDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync((CategoryOutputModel)null);

            var result = await controller.Edit(999);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            var categoryDTO = new CategoryUpdateDTO { Id = 1, Name = "Updated" };

            mockCategoryService
                .Setup(s => s.UpdateCategoryAsync(categoryDTO))
                .Returns(Task.CompletedTask);

            var result = await controller.Edit(1, categoryDTO);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Edit_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            controller.ModelState.AddModelError("Name", "Required");
            var categoryDTO = new CategoryUpdateDTO { Id = 1, Name = "Updated" };

            var result = await controller.Edit(1, categoryDTO);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(categoryDTO, viewResult.Model);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithCategory()
        {
            var category = new CategoryOutputModel { Id = 1, Name = "Adventure" };

            mockCategoryService
                .Setup(s => s.GetCategoryDetailsAsync(1))
                .ReturnsAsync(category);

            var result = await controller.Details(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as CategoryOutputModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [Test]
        public async Task Delete_Get_ReturnsViewResult_WithCategory()
        {
            var category = new CategoryOutputModel { Id = 1, Name = "Adventure" };

            mockCategoryService
                .Setup(s => s.GetCategoryDetailsAsync(1))
                .ReturnsAsync(category);

            var result = await controller.Delete(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as CategoryOutputModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [Test]
        public async Task DeleteConfirmed_Post_RedirectsToIndex()
        {
            mockCategoryService
                .Setup(s => s.DeleteCategoryAsync(1))
                .Returns(Task.CompletedTask);

            var result = await controller.DeleteConfirmed(1);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }
    }
}
