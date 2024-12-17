using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using VenomGames.Controllers;
using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;
using VenomGames.Models.ApplicationUser;

namespace VenomGames.Tests.Controllers
{
    [TestFixture]
    public class ApplicationUserControllerTests
    {
        private Mock<IApplicationUserService> mockUserService;
        private Mock<IShoppingCartService> mockCartService;
        private Mock<SignInManager<ApplicationUser>> mockSignInManager;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private ApplicationUserController controller;

        [SetUp]
        public void Setup()
        {
            mockUserService = new Mock<IApplicationUserService>();
            mockCartService = new Mock<IShoppingCartService>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                null, null, null, null);

            controller = new ApplicationUserController(
                mockUserService.Object,
                mockSignInManager.Object,
                mockUserManager.Object,
                mockCartService.Object);

            SetupUrlHelper();
        }


        [TearDown]
        public void TearDown()
        {
            controller.Dispose();
        }


        [Test]
        public async Task Index_ReturnsViewWithUsers()
        {
            SetupHttpContext(isAuthenticated: true);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "TestUser1" },
                new ApplicationUser { UserName = "TestUser2" }
            };

            mockUserService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(users);
            mockCartService.Setup(s => s.GetCartItemCountAsync(It.IsAny<string>())).ReturnsAsync(5);

            var result = await controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOf<IEnumerable<ApplicationUser>>(viewResult.Model);
            Assert.AreEqual(2, ((List<ApplicationUser>)viewResult.Model).Count);
            Assert.AreEqual(5, controller.ViewBag.CartCount);
        }

        [Test]
        public async Task LoginPost_WithValidModel_RedirectsToLocal()
        {
            SetupHttpContext();
            var loginModel = new LoginViewModel { Email = "test@example.com", Password = "Password123", RememberMe = false };

            mockSignInManager
                .Setup(manager => manager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var result = await controller.Login(loginModel, "/Home/Index");

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("/Home/Index", redirectResult.Url);
        }

        [Test]
        public async Task Register_WithValidModel_RedirectsToLocal()
        {
            SetupHttpContext();
            var registerModel = new RegisterViewModel
            {
                Email = "newuser@example.com",
                Password = "StrongPassword123",
                ConfirmPassword = "StrongPassword123"
            };

            mockUserManager
                .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), registerModel.Password))
                .ReturnsAsync(IdentityResult.Success);

            mockSignInManager.Setup(sm => sm.SignInAsync(It.IsAny<ApplicationUser>(), false, null))
                .Returns(Task.CompletedTask);

            var result = await controller.Register(registerModel, "/Home/Index");

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("/Home/Index", redirectResult.Url);
        }

        [Test]
        public async Task Logout_ReturnsRedirectToHome()
        {
            mockSignInManager
                .Setup(sm => sm.SignOutAsync())
                .Returns(Task.CompletedTask);

            var httpContextMock = new Mock<HttpContext>();
            var tempData = new TempDataDictionary(httpContextMock.Object, Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            var result = await controller.Logout();

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }


        [Test]
        public async Task Details_ValidId_ReturnsViewWithUser()
        {
            var userId = "1";
            var user = new ApplicationUser { Id = userId, UserName = "User1" };
            mockUserService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var result = await controller.Details(userId) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(user, result.Model);
        }

        [Test]
        public async Task Edit_Post_ValidId_RedirectsToIndex()
        {
            var userId = "1";
            var user = new ApplicationUser { Id = userId, UserName = "User1" };
            mockUserService.Setup(s => s.UpdateUserAsync(user)).Returns(Task.CompletedTask);

            var result = await controller.Edit(userId, user) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Edit_Post_InvalidId_ReturnsNotFound()
        {
            var userId = "1";
            var user = new ApplicationUser { Id = "2", UserName = "User1" };

            var result = await controller.Edit(userId, user);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ValidId_ReturnsViewWithUser()
        {
            var userId = "1";
            var user = new ApplicationUser { Id = userId, UserName = "User1" };
            mockUserService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var result = await controller.Delete(userId) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(user, result.Model);
        }

        [Test]
        public async Task DeleteConfirmed_ValidId_RedirectsToIndex()
        {
            var userId = "1";
            mockUserService.Setup(s => s.DeleteUserAsync(userId)).Returns(Task.CompletedTask);

            var result = await controller.DeleteConfirmed(userId) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task GetByEmail_ValidEmail_ReturnsViewWithUser()
        {
            var email = "test@example.com";
            var user = new ApplicationUser { Id = "1", UserName = "User1", Email = email };
            mockUserService.Setup(s => s.GetUserByEmailAsync(email)).ReturnsAsync(user);

            var result = await controller.GetByEmail(email) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(user, result.Model);
        }

        [Test]
        public async Task GetByEmail_InvalidEmail_ReturnsNotFound()
        {
            var email = "nonexistent@example.com";
            mockUserService.Setup(s => s.GetUserByEmailAsync(email)).ReturnsAsync((ApplicationUser)null);

            var result = await controller.GetByEmail(email);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        private void SetupHttpContext(bool isAuthenticated = true, string userId = "test-user-id")
        {
            var claims = isAuthenticated
                ? new[] { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId) }
                : new System.Security.Claims.Claim[0];

            var claimsIdentity = new System.Security.Claims.ClaimsIdentity(claims, "mock");
            var claimsPrincipal = new System.Security.Claims.ClaimsPrincipal(claimsIdentity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.User).Returns(claimsPrincipal);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            mockUserManager
                .Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .Returns(userId);
        }

        private void SetupUrlHelper()
        {
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(url => url.IsLocalUrl(It.IsAny<string>())).Returns(true);
            controller.Url = mockUrlHelper.Object;
        }
    }
}
