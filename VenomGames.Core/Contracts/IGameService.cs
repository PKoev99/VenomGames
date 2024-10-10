using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Contracts
{
    /// <summary>
    /// Interface for Game service.
    /// Defines methods for game management.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Retrieves all games.
        /// </summary>
        /// <returns>List of all games.</returns>
        IEnumerable<Game> GetAllGames();

        /// <summary>
        /// Retrieves a game by its ID.
        /// </summary>
        /// <param name="id">The ID of the game.</param>
        /// <returns>A single game.</returns>
        Game GetGameById(int id);

        /// <summary>
        /// Adds a new game to the database.
        /// </summary>
        /// <param name="game">Game to be added.</param>
        void CreateGame(Game game);

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        /// <param name="game">Game with updated information.</param>
        void UpdateGame(Game game);

        /// <summary>
        /// Deletes a game by its ID.
        /// </summary>
        /// <param name="id">ID of the game to be deleted.</param>
        void DeleteGame(int id);
    }
}
