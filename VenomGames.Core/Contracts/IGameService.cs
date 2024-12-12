using VenomGames.Core.DTOs.Category;
using VenomGames.Core.DTOs.Game;

namespace VenomGames.Core.Contracts
{
    /// <summary>
    /// Interface for Game service.
    /// Defines methods for game management.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Searches for games from the database.
        /// </summary>
        /// <returns>List of all games.</returns>
        Task<IEnumerable<GameOutputModel>> GetGamesAsync(GetGamesQuery queery);

        /// <summary>
        /// Retrieves all games.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GameOutputModel>> GetAllGamesAsync();

        /// <summary>
        /// Retrieves details about a specific game by ID.
        /// </summary>
        /// <param name="id">The ID of the game.</param>
        /// <returns>A single game.</returns>
        Task<GameOutputModel> GetGameDetailsAsync(int id);

        /// <summary>
        /// Retrieves the featured games by rating.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GameOutputModel>> GetFeaturedGamesAsync();

        /// <summary>
        /// Gets all games by a specific category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<GameOutputModel>> GetGamesByCategoryAsync(int id);


        /// <summary>
        /// Adds a new game to the database.
        /// </summary>
        /// <param name="game">Game to be added.</param>
        Task CreateGameAsync(GameCreateDTO game);

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        /// <param name="game">Game with updated information.</param>
        Task UpdateGameAsync(GameOutputModel game);

        /// <summary>
        /// Deletes a game by its ID.
        /// </summary>
        /// <param name="id">ID of the game to be deleted.</param>
        Task DeleteGameAsync(int id);
    }
}