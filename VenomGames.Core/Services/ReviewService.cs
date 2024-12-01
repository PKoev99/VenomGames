using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service for managing reviews.
    /// </summary>
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepository;

        public ReviewService(IReviewRepository _reviewRepository)
        {
            reviewRepository = _reviewRepository;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await reviewRepository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves all reviews for a specific game.
        /// </summary>
        public async Task<IEnumerable<Review>> GetReviewsByGameIdAsync(int gameId)
        {
            return await reviewRepository.GetReviewsByGameIdAsync(gameId);  
        }

        /// <summary>
        /// Retrieves a review by ID.
        /// </summary>
        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await reviewRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Adds a new review to the repository.
        /// </summary>
        public async Task CreateReviewAsync(Review review)
        {
            await reviewRepository.AddAsync(review);
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        public async Task UpdateReviewAsync(Review review)
        {
            await reviewRepository.UpdateAsync(review);
        }

        /// <summary>
        /// Deletes a review by ID.
        /// </summary>
        public async Task DeleteReviewAsync(int id)
        {
            await reviewRepository.DeleteAsync(id);
        }

       
    }
}
