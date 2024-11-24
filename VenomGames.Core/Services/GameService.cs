using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Game;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service class for managing games.
    /// </summary>
    public class GameService : IGameService
    {
        private readonly IRepository<Game> gameRepository;

        public GameService(IRepository<Game> _gameRepository)
        {
            gameRepository = _gameRepository;
        }

        /// <summary>
        /// Retrieves all games from the repository.
        /// </summary>
        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await gameRepository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a specific game by ID.
        /// </summary>
        public async Task<Game> GetGameByIdAsync(int id)
        {
            return await gameRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Adds a new game to the repository.
        /// </summary>
        public async Task AddGameAsync(Game game)
        {
            await gameRepository.AddAsync(game);
        }

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        async Task IGameService.UpdateGameAsync(Game game)
        {
            await gameRepository.UpdateAsync(game);
        }

        /// <summary>
        /// Deletes a game by ID.
        /// </summary>
        async Task IGameService.DeleteGameAsync(int id)
        {
            await gameRepository.DeleteAsync(id);
        }
    }
}