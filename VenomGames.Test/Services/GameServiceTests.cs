using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.DTOs.Game;
using VenomGames.Core.Services;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Services
{
    [TestFixture]
    public class GameServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options;
        private ApplicationDbContext context;
        private GameService gameService;

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(options);
            gameService = new GameService(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public async Task GetGamesAsync_ReturnsFilteredGames_WhenSearchQueryIsProvided()
        {
            context.Games.AddRange(new List<Game>
            {
                new Game { Id = 1, Title = "Game One", Description = "First game", Price = 10, ImageUrl = "url1" },
                new Game { Id = 2, Title = "Another Game", Description = "Second game", Price = 20, ImageUrl = "url2" }
            });
            await context.SaveChangesAsync();

            var result = await gameService.GetGamesAsync(1,10, "Another");

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Another Game", result.First().Title);
        }

        [Test]
        public async Task GetGamesAsync_ReturnsFilteredGames_WhenSearchQueryIsNotProvided()
        {
            context.Games.AddRange(new List<Game>
            {
                new Game { Id = 1, Title = "Game One", Description = "First game", Price = 10, ImageUrl = "url1" },
                new Game { Id = 2, Title = "Another Game", Description = "Second game", Price = 20, ImageUrl = "url2" },
                new Game { Id = 3, Title = "And Another Game", Description = "Third game", Price = 30, ImageUrl = "url3" }
            });
            await context.SaveChangesAsync();

            var result = await gameService.GetGamesAsync(1, 10,"");

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("Game One", result.First().Title);
        }

        [Test]
        public async Task GetTotalGamesAsync_ReturnsCorrectCount_WhenSearchQueryIsProvided()
        {
            context.Games.AddRange(new List<Game>
            {
                new Game { Id = 1, Title = "Game One", Description = "First game" },
                new Game { Id = 2, Title = "Another Game", Description = "Second game" },
                new Game { Id = 3, Title = "Another One", Description = "Third one" }
            });
            await context.SaveChangesAsync();

            var result = await gameService.GetTotalGamesAsync("Game");

            Assert.AreEqual(2, result);
        }

        [Test]
        public async Task GetTotalGamesAsync_ReturnsCorrectCount_WhenSearchQueryIsNotProvided()
        {
            context.Games.AddRange(new List<Game>
            {
                new Game { Id = 1, Title = "Game One", Description = "First game" },
                new Game { Id = 2, Title = "Another Game", Description = "Second game" },
                new Game { Id = 3, Title = "Another One", Description = "Third one" }
            });
            await context.SaveChangesAsync();

            var result = await gameService.GetTotalGamesAsync("");

            Assert.AreEqual(3, result);
        }

        [Test]
        public async Task GetAllGamesAsync_ReturnsAllGames()
        {
            context.Games.AddRange(new List<Game>
            {
                new Game { Id = 1, Title = "Game One", Description = "First game" },
                new Game { Id = 2, Title = "Another Game", Description = "Second game" },
                new Game { Id = 3, Title = "Another One", Description = "Third one" }
            });
            await context.SaveChangesAsync();

            var result = await gameService.GetAllGamesAsync();

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task GetGameDetailsAsync_ReturnsGameDetails_WhenGameExists()
        {
            context.Games.AddRange(new List<Game>
            {
                new Game { Id = 1, Title = "Game One", Description = "First game" },
                new Game { Id = 2, Title = "Another Game", Description = "Second game" },
                new Game { Id = 3, Title = "Another One", Description = "Third one" }
            });
            await context.SaveChangesAsync();

            var result = await gameService.GetGameDetailsAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Game One", result.Title);
        }

        [Test]
        public void GetGameDetailsAsync_ThrowsNotFoundException_WhenGameDoesNotExist()
        {
            Assert.ThrowsAsync<NotFoundException>(() => gameService.GetGameDetailsAsync(1));
        }

        [Test]
        public async Task CreateGameAsync_AddsNewGameToDatabase()
        {
            var gameDto = new GameCreateDTO
            {
                Title = "New Game",
                Price = 15,
                Description = "New game description",
                ImageUrl = "newUrl",
                SelectedCategoryIds = new List<int> { 1, 2 }
            };

            await gameService.CreateGameAsync(gameDto);

            var createdGame = await context.Games.FirstOrDefaultAsync();
            Assert.IsNotNull(createdGame);
            Assert.AreEqual(gameDto.Title, createdGame.Title);
            Assert.AreEqual(gameDto.Price, createdGame.Price);
            Assert.AreEqual(gameDto.Description, createdGame.Description);
            Assert.AreEqual(gameDto.ImageUrl, createdGame.ImageUrl);
        }

        [Test]
        public async Task DeleteGameAsync_RemovesGameFromDatabase_WhenGameExists()
        {
            context.Games.AddRange(new List<Game>
            {
                new Game { Id = 1, Title = "Game One", Description = "First game" },
                new Game { Id = 2, Title = "Another Game", Description = "Second game" },
                new Game { Id = 3, Title = "Another One", Description = "Third one" }
            });
            await context.SaveChangesAsync();

            await gameService.DeleteGameAsync(1);

            var deletedGame = await context.Games.FindAsync(1);
            Assert.IsNull(deletedGame);
        }

        [Test]
        public void DeleteGameAsync_ThrowsNotFoundException_WhenGameDoesNotExist()
        {
            Assert.ThrowsAsync<NotFoundException>(() => gameService.DeleteGameAsync(1));
        }
    }
}
