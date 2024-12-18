using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.Services;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Services
{
    [TestFixture]
    public class ApplicationUserServiceTests
    {
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private ApplicationUserService service;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            service = new ApplicationUserService(mockUserManager.Object, null);
        }

        [Test]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", UserName = "user1" },
                new ApplicationUser { Id = "2", UserName = "user2" }
            };

            var mockUserQueryable = userList.AsQueryable().BuildMock();

            mockUserManager.Setup(m => m.Users).Returns(mockUserQueryable.Object);

            var result = await service.GetAllUsersAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(u => u.UserName == "user1"));
            Assert.IsTrue(result.Any(u => u.UserName == "user2"));
        }

        [Test]
        public async Task GetUserByEmailAsync_WhenUserExists_ShouldReturnUser()
        {
            var email = "test@example.com";
            var user = new ApplicationUser { Email = email, UserName = "TestUser" };

            mockUserManager.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(user);

            var result = await service.GetUserByEmailAsync(email);

            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
            mockUserManager.Verify(m => m.FindByEmailAsync(email), Times.Once);
        }

        [Test]
        public void GetUserByEmailAsync_WhenUserDoesNotExist_ShouldThrowNotFoundException()
        {
            var email = "nonexistent@example.com";
            mockUserManager.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync((ApplicationUser)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await service.GetUserByEmailAsync(email));
            Assert.AreEqual($"Entity \"ApplicationUser\" ({email}) was not found.", ex.Message);
        }

        [Test]
        public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnUser()
        {
            var id = "123";
            var user = new ApplicationUser { Id = id, UserName = "TestUser" };

            mockUserManager.Setup(m => m.FindByIdAsync(id)).ReturnsAsync(user);

            var result = await service.GetUserByIdAsync(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            mockUserManager.Verify(m => m.FindByIdAsync(id), Times.Once);
        }

        [Test]
        public void GetUserByIdAsync_WhenUserDoesNotExist_ShouldThrowNotFoundException()
        {
            var id = "invalid_id";
            mockUserManager.Setup(m => m.FindByIdAsync(id)).ReturnsAsync((ApplicationUser)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await service.GetUserByIdAsync(id));
            Assert.AreEqual($"Entity \"ApplicationUser\" ({id}) was not found.", ex.Message);
        }

        [Test]
        public async Task RegisterUserAsync_ShouldCreateUser()
        {
            var user = new ApplicationUser { UserName = "NewUser" };
            var password = "Password123!";
            var identityResult = IdentityResult.Success;

            mockUserManager.Setup(m => m.CreateAsync(user, password)).ReturnsAsync(identityResult);

            var result = await service.RegisterUserAsync(user, password);

            Assert.IsTrue(result.Succeeded);
            mockUserManager.Verify(m => m.CreateAsync(user, password), Times.Once);
        }

        [Test]
        public async Task AddUserToRoleAsync_ShouldAddUserToRole()
        {
            var user = new ApplicationUser { UserName = "User" };
            var role = "Admin";
            var identityResult = IdentityResult.Success;

            mockUserManager.Setup(m => m.AddToRoleAsync(user, role)).ReturnsAsync(identityResult);

            var result = await service.AddUserToRoleAsync(user, role);

            Assert.IsTrue(result.Succeeded);
            mockUserManager.Verify(m => m.AddToRoleAsync(user, role), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            var user = new ApplicationUser { UserName = "UpdatedUser" };

            mockUserManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            await service.UpdateUserAsync(user);

            mockUserManager.Verify(m => m.UpdateAsync(user), Times.Once);
        }

        [Test]
        public async Task DeleteUserAsync_WhenUserExists_ShouldDeleteUser()
        {
            var id = "123";
            var user = new ApplicationUser { Id = id };

            mockUserManager.Setup(m => m.FindByIdAsync(id)).ReturnsAsync(user);
            mockUserManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            await service.DeleteUserAsync(id);

            mockUserManager.Verify(m => m.DeleteAsync(user), Times.Once);
        }

        [Test]
        public void DeleteUserAsync_WhenUserDoesNotExist_ShouldThrowNotFoundException()
        {
            var id = "invalid_id";
            mockUserManager.Setup(m => m.FindByIdAsync(id)).ReturnsAsync((ApplicationUser)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await service.DeleteUserAsync(id));
            Assert.AreEqual($"Entity \"ApplicationUser\" ({id}) was not found.", ex.Message);
        }
    }
}