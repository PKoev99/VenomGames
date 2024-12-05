using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Contracts
{
    /// <summary>
    /// Interface for ApplicationUser service.
    /// Defines methods for user management.
    /// </summary>
    public interface IApplicationUserService
    {
        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>List of all users.</returns>
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A single user.</returns>
        Task<ApplicationUser?> GetUserByIdAsync(string id);

        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>A single user.</returns>
        Task<ApplicationUser?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="user">User with updated information.</param>
        Task UpdateUserAsync(ApplicationUser user);

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">ID of the user to be deleted.</param>
        Task DeleteUserAsync(string user);
    }
}
