using VenomGames.Core.DTOs.Game;
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
        Task<IEnumerable<Game>> GetAllGamesAsync();

        /// <summary>
        /// Retrieves a game by its ID.
        /// </summary>
        /// <param name="id">The ID of the game.</param>
        /// <returns>A single game.</returns>
        Task<Game> GetGameByIdAsync(int id);

        /// <summary>
        /// Adds a new game to the database.
        /// </summary>
        /// <param name="game">Game to be added.</param>
        Task AddGameAsync(Game game);

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        /// <param name="game">Game with updated information.</param>
        Task UpdateGameAsync(Game game);

        /// <summary>
        /// Deletes a game by its ID.
        /// </summary>
        /// <param name="id">ID of the game to be deleted.</param>
        Task DeleteGameAsync(int id);
    }
}