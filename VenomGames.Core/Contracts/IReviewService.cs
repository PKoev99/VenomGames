using VenomGames.Core.DTOs.Review;

namespace VenomGames.Core.Contracts
{
    /// <summary>
    /// Interface for Review service.
    /// Defines methods for review management.
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Retrieves all reviews.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReviewOutputModel>> GetReviewsAsync();

        /// <summary>
        /// Retrieves all reviews for a specific game.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <returns>List of reviews for the game.</returns>
        Task<IEnumerable<ReviewOutputModel>> GetReviewsByGameIdAsync(int gameId);

        /// <summary>
        /// Retrieves a review by its ID.
        /// </summary>
        /// <param name="id">The ID of the review.</param>
        /// <returns>A single review.</returns>
        Task<ReviewOutputModel> GetReviewDetailsAsync(int id);

        /// <summary>
        /// Adds a new review to the database.
        /// </summary>
        /// <param name="review">Review to be added.</param>
        Task CreateReviewAsync(ReviewCreateDTO review, string userId);

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="review">Review with updated information.</param>
        Task UpdateReviewAsync(ReviewUpdateDTO review);

        /// <summary>
        /// Deletes a review by its ID.
        /// </summary>
        /// <param name="id">ID of the review to be deleted.</param>
        Task DeleteReviewAsync(int id);
    }
}
