using VenomGames.Core.Contracts;
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
        public IEnumerable<Game> GetAllGames()
        {
            return gameRepository.GetAll();
        }

        /// <summary>
        /// Retrieves a specific game by ID.
        /// </summary>
        public Game GetGameById(int id)
        {
            return gameRepository.GetById(id);
        }

        /// <summary>
        /// Adds a new game to the repository.
        /// </summary>
        public void CreateGame(Game game)
        {
            gameRepository.Add(game);
        }

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        public void UpdateGame(Game game)
        {
            gameRepository.Update(game);
        }

        /// <summary>
        /// Deletes a game by ID.
        /// </summary>
        public void DeleteGame(int id)
        {
            gameRepository.Delete(id);
        }
    }
}
