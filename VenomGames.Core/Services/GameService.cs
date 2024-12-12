using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Game;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service class for managing games.
    /// </summary>
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext context;

        public GameService(ApplicationDbContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// Searches for games from the database.
        /// </summary>
        public async Task<IEnumerable<GameOutputModel>> GetGamesAsync(GetGamesQuery query)
        {
            IQueryable<Game> games = context.Games;

            string? gameTitle = query.Title;
            if (!gameTitle.IsNullOrEmpty())
            {
                games = games.Where(g => g.Title.Contains(gameTitle!));
            }

            IEnumerable<GameOutputModel> gamesOutput = await games
                .Select(g => new GameOutputModel
                {
                    GameId = g.Id,
                    Description = g.Description,
                    Price = g.Price,
                    Title = g.Title,
                    GameCategories = g.GameCategories,
                    Reviews = g.Reviews,
                    ImageUrl = g.ImageUrl
                }).ToListAsync();

            return gamesOutput;

        }

        /// <summary>
        /// Retrieves all games.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GameOutputModel>> GetAllGamesAsync()
        {
            IEnumerable<Game> games = await context.Games.ToListAsync();

            return games.Select(c => new GameOutputModel
            {
                GameId = c.Id,
                Description = c.Description,
                Price = c.Price,
                Title = c.Title,
                GameCategories = c.GameCategories,
                Reviews = c.Reviews,
                ImageUrl = c.ImageUrl
            });
        }

        /// <summary>
        /// Retrieves details about a specific game by ID.
        /// </summary>
        public async Task<GameOutputModel> GetGameDetailsAsync(int id)
        {
            var game = await context.Games
                    .Where(g => g.Id == id)
                    .Include(g=>g.GameCategories)
                    .ThenInclude(gc=>gc.Category)
                    .Select(g => new GameOutputModel
                    {
                        GameId = g.Id,
                        Title = g.Title,
                        Price = g.Price,
                        Description = g.Description,
                        ImageUrl = g.ImageUrl,
                        GameCategories = g.GameCategories,
                        Reviews = g.Reviews
                    })
                    .FirstOrDefaultAsync();

            if (game == null)
            {
                throw new NotFoundException(nameof(Game), id);
            }

            return game;
        }

        /// <summary>
        /// Adds a new game to the database.
        /// </summary>
        public async Task CreateGameAsync(GameCreateDTO game)
        {
            var newGame = new Game
            {
                Title = game.Title,
                Price = game.Price,
                Description = game.Description,
                ImageUrl = game.ImageUrl
            };

            // Add relationships with the selected categories
            foreach (var categoryId in game.SelectedCategoryIds)
            {
                newGame.GameCategories.Add(new GameCategory
                {
                    CategoryId = categoryId
                });
            }

            context.Games.Add(newGame);
            await context.SaveChangesAsync();
        }



        /// <summary>
        /// Updates an existing game.
        /// </summary>
        public async Task UpdateGameAsync(GameOutputModel game)
        {
            var existingGame = await context.Games.FirstOrDefaultAsync(g => g.Id == game.GameId);
            if (existingGame != null)
            {
                existingGame.Title = game.Title;
                existingGame.Price = game.Price;
                existingGame.Description = game.Description;
                existingGame.ImageUrl = game.ImageUrl;

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a game by ID.
        /// </summary>
        public async Task DeleteGameAsync(int id)
        {
            var game = await context.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                throw new NotFoundException(nameof(Game), id);
            }

            context.Games.Remove(game);
            await context.SaveChangesAsync();
        }


        /// <summary>
        /// Retrieves featured games by rating.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GameOutputModel>> GetFeaturedGamesAsync()
        {
            IEnumerable<Game> games = await context.Games
                .Where(g => g.Reviews.Any(x => x.Rating >= 3))
                .OrderByDescending(g => g.Reviews.Average(x => x.Rating))
                .Take(5)
                .ToListAsync();

            return games.Select(g => new GameOutputModel
            {
                GameId = g.Id,
                Title = g.Title,
                Price = g.Price,
                Description = g.Description,
                GameCategories = g.GameCategories,
                Reviews = g.Reviews,
                ImageUrl = g.ImageUrl
            });
        }

        /// <summary>
        /// Retrieves games by a certain category.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GameOutputModel>> GetGamesByCategoryAsync(int categoryId)
        {
            return await context.Games
                .Where(g => g.GameCategories.Any(gc => gc.CategoryId == categoryId))
                .Select(g => new GameOutputModel
                {
                    GameId = g.Id,
                    Title = g.Title,
                    Price = g.Price,
                    Description = g.Description,
                    GameCategories = g.GameCategories,
                    Reviews = g.Reviews,
                    ImageUrl = g.ImageUrl
                }).ToListAsync();
        }
    }
}