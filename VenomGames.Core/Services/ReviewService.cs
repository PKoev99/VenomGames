using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Review;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service for managing reviews.
    /// </summary>
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext context;

        public ReviewService(ApplicationDbContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// Searches for reviews from the database.
        /// </summary>
        public async Task<IEnumerable<ReviewOutputModel>> GetReviewsAsync(GetReviewsQuery query)
        {
            IQueryable<Review> reviews = context.Reviews;

            int? gameId = query.GameId;
            if (gameId.HasValue)
            {
                reviews = reviews.Where(r => r.GameId == gameId);
            }

            string? userId = query.UserId;
            if (!userId.IsNullOrEmpty())
            {
                reviews = reviews.Where(r => r.UserId == userId);
            }


            decimal? rating = query.Rating;
            if (rating.HasValue)
            {
                reviews = reviews.Where(r => r.Rating == rating);
            }

            DateTime? startDate = query.StartDate;
            DateTime? endDate = query.EndDate;
            if (startDate.HasValue && endDate.HasValue)
            {
                reviews = reviews.Where(o => o.CreatedAt >= startDate.Value.Date && o.CreatedAt <= endDate.Value.Date);
            }

            IEnumerable<ReviewOutputModel> reviewsOutput = await reviews
                .Select(r => new ReviewOutputModel
                {
                    GameId = r.GameId,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Content = r.Content,
                    CreatedAt = r.CreatedAt
                }).ToListAsync();

            return reviewsOutput;

        }

        /// <summary>
        /// Get all reviews for a specific game by its ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<ReviewOutputModel>> GetReviewsByGameIdAsync(int gameId)
        {
            IEnumerable<ReviewOutputModel> reviewsOutput = await context.Reviews
                .Where(r => r.GameId == gameId)
                .Select(r => new ReviewOutputModel
                {
                    GameId = r.GameId,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Content = r.Content,
                    CreatedAt = r.CreatedAt
                }).ToListAsync();

            return reviewsOutput;

        }

        /// <summary>
        /// Retrieves details about a specific review by ID.
        /// </summary>
        public async Task<ReviewOutputModel> GetReviewDetailsAsync(int id)
        {
            ReviewOutputModel? review = await context.Reviews
                .Where(r => r.ReviewId == id)
                .Select(r => new ReviewOutputModel
                {
                    GameId = r.GameId,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Content = r.Content,
                    CreatedAt = r.CreatedAt
                }).FirstOrDefaultAsync();

            if (review == null)
            {
                throw new NotFoundException(nameof(Review), id);
            }

            return review;
        }

        /// <summary>
        /// Adds a new review to the database.
        /// </summary>
        public async Task CreateReviewAsync(ReviewCreateDTO review)
        {
            Review newReview = new Review()
            {
                GameId = review.GameId,
                Rating = review.Rating,
                Content = review.Content,
                CreatedAt = review.CreatedAt,
                UserId = review.UserId
            };

            context.Reviews.Add(newReview);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        public async Task UpdateReviewAsync(ReviewUpdateDTO review)
        {
            Review newOrder = new Review()
            {
                GameId = review.GameId,
                Rating = review.Rating,
                Content = review.Content,
                CreatedAt = review.CreatedAt,
                UserId = review.UserId
            };

            context.Reviews.Update(newOrder);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a review by ID.
        /// </summary>
        public async Task DeleteReviewAsync(int id)
        {
            Review? review = await context.Reviews.FirstOrDefaultAsync(o => o.ReviewId == id);

            if (review == null)
            {
                throw new NotFoundException(nameof(Review), id);
            }

            context.Reviews.Remove(review);
            await context.SaveChangesAsync();
        }
    }
}
