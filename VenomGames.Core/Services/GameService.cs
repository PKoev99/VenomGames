using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Game;
using VenomGames.Core.DTOs.Review;
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
        public async Task<IEnumerable<GameOutputModel>> GetGamesAsync(int page, int pageSize, string searchQuery)
        {
            IQueryable<Game> games = context.Games;

            if (!searchQuery.IsNullOrEmpty())
            {
                games = games.Where(g => g.Title.Contains(searchQuery) || g.Description.Contains(searchQuery));
            }

            IEnumerable<GameOutputModel> gamesOutput = await games
                .Select(g => new GameOutputModel
                {
                    GameId = g.Id,
                    Description = g.Description,
                    Price = g.Price,
                    Title = g.Title,
                    GameCategories = g.GameCategories,
                    Reviews = g.Reviews.Select(r => new ReviewOutputModel
                    {
                        ReviewId = r.ReviewId,
                        GameId = r.GameId,
                        Content = r.Content,
                        Rating = r.Rating,
                        UserName = r.User.UserName,
                        CreatedAt = r.CreatedAt
                    }).ToList(),
                    ImageUrl = g.ImageUrl
                }).ToListAsync();

            return gamesOutput;

        }

        /// <summary>
        /// Get the amount of all games. 
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public async Task<int> GetTotalGamesAsync(string searchQuery)
        {
            var games = context.Games.AsQueryable();

            if (!searchQuery.IsNullOrEmpty())
            {
                games = games.Where(g => g.Title.Contains(searchQuery) || g.Description.Contains(searchQuery));
            }

            return await games.CountAsync();
        }


        /// <summary>
        /// Retrieves all games.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GameOutputModel>> GetAllGamesAsync()
        {
            var games = await context.Games
                .Include(g => g.Reviews)
                .Select(g => new GameOutputModel
                {
                    GameId = g.Id,
                    Title = g.Title,
                    Price = g.Price,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl,
                    AverageRating = g.Reviews.Any() ? g.Reviews.Average(r => r.Rating) : 0,
                    Reviews = g.Reviews.Select(r => new ReviewOutputModel
                    {
                        ReviewId = r.ReviewId,
                        GameId = r.GameId,
                        Content = r.Content,
                        Rating = r.Rating,
                        UserName = r.User.UserName,
                        CreatedAt = r.CreatedAt
                    }).ToList()
                })
                .ToListAsync();

            return games;
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
                        Reviews = g.Reviews.Select(r => new ReviewOutputModel
                        {
                            ReviewId = r.ReviewId,
                            GameId = r.GameId,
                            Content = r.Content,
                            Rating = r.Rating,
                            UserName = r.User.UserName,
                            CreatedAt = r.CreatedAt
                        }).ToList(),
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
            var games = await context.Games
                .Where(g => g.Reviews.Any())
                .Select(g => new
                {
                    Game = g,
                    AverageRating = g.Reviews.Average(r => r.Rating)
                })
                .Where(g => g.AverageRating >= 7)
                .ToListAsync();

            var gameOutputModels = games.Select(g => new GameOutputModel
            {
                GameId = g.Game.Id,
                Title = g.Game.Title,
                Description = g.Game.Description,
                Price = g.Game.Price,
                ImageUrl = g.Game.ImageUrl,
                AverageRating = g.AverageRating,
                GameCategories = g.Game.GameCategories,
                Reviews = g.Game.Reviews.Select(r => new ReviewOutputModel
                {
                    ReviewId = r.ReviewId,
                    GameId = r.GameId,
                    GameTitle = r.Game.Title,
                    UserId = r.UserId,
                    UserName = r.User.UserName,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt
                }).ToList()
            }).ToList();

            return gameOutputModels;
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
                    Reviews = g.Reviews.Select(r => new ReviewOutputModel
                    {
                        ReviewId = r.ReviewId,
                        GameId = r.GameId,
                        Content = r.Content,
                        Rating = r.Rating,
                        UserName = r.User.UserName,
                        CreatedAt = r.CreatedAt
                    }).ToList(),
                    ImageUrl = g.ImageUrl
                }).ToListAsync();
        }
    }
}