using Moq;
using VenomGames.Core.Contracts;
using VenomGames.Core.Services;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;
using VenomGames.Test.Common;

namespace VenomGames.Test.Services
{
    [TestFixture]
    public class GameServiceTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private GameService _gameService;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>();

            var games = new List<Game>
            {
                new Game { Id = 1, Title = "Game 1", Price = 19.99m, Description = "Game 1 Description", ImageUrl = "image1.png" },
                new Game { Id = 2, Title = "Game 2", Price = 29.99m, Description = "Game 2 Description", ImageUrl = "image2.png" }
            }.AsQueryable();

            _mockContext.Setup(c => c.Games).Returns(DbSetMockHelper.MockDbSet(games).Object);

            _gameService = new GameService(_mockContext.Object);
        }


        [Test]
        public async Task GetGamesAsync_ShouldReturnGames()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var searchQuery = string.Empty;

            // Act
            var result = await _gameService.GetGamesAsync(page, pageSize, searchQuery);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }


    }
}
