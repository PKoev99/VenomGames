using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service for managing reviews.
    /// </summary>
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> reviewRepository;

        public ReviewService(IRepository<Review> _reviewRepository)
        {
            reviewRepository = _reviewRepository;
        }

        /// <summary>
        /// Retrieves all reviews for a specific game.
        /// </summary>
        public IEnumerable<Review> GetReviewsByGameId(int gameId)
        {
            return reviewRepository.GetAll().Where(r => r.GameId == gameId);
        }

        /// <summary>
        /// Retrieves a review by ID.
        /// </summary>
        public Review GetReviewById(int id)
        {
            return reviewRepository.GetById(id);
        }

        /// <summary>
        /// Adds a new review to the repository.
        /// </summary>
        public void CreateReview(Review review)
        {
            reviewRepository.Add(review);
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        public void UpdateReview(Review review)
        {
            reviewRepository.Update(review);
        }

        /// <summary>
        /// Deletes a review by ID.
        /// </summary>
        public void DeleteReview(int id)
        {
            reviewRepository.Delete(id);
        }
    }
}
