using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.DTOs.Review;
using VenomGames.Core.Services;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Tests.Services
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private ApplicationDbContext context;
        private ReviewService reviewService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(options);
            reviewService = new ReviewService(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task GetReviewsAsync_ReturnsAllReviews()
        {
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, GameId = 1, UserId = "user1", Content = "Great game!", Rating = 5, CreatedAt = DateTime.UtcNow },
                new Review { ReviewId = 2, GameId = 2, UserId = "user2", Content = "Not bad", Rating = 3, CreatedAt = DateTime.UtcNow }
            };

            await context.Reviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();

            var result = await reviewService.GetReviewsAsync();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Great game!", result.First().Content);
        }

        [Test]
        public async Task GetReviewsByGameIdAsync_ReturnsReviewsForSpecificGame()
        {
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, GameId = 1, UserId = "user1", Content = "Amazing!", Rating = 5, CreatedAt = DateTime.UtcNow },
                new Review { ReviewId = 2, GameId = 2, UserId = "user2", Content = "Decent", Rating = 3, CreatedAt = DateTime.UtcNow }
            };

            await context.Reviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();

            var result = await reviewService.GetReviewsByGameIdAsync(1);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Amazing!", result.First().Content);
        }

        [Test]
        public async Task GetReviewDetailsAsync_ValidId_ReturnsReviewDetails()
        {
            var review = new Review { ReviewId = 1, GameId = 1, UserId = "user1", Content = "Fantastic!", Rating = 4, CreatedAt = DateTime.UtcNow };
            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync();

            var result = await reviewService.GetReviewDetailsAsync(1);

            Assert.NotNull(result);
            Assert.AreEqual("Fantastic!", result.Content);
        }

        [Test]
        public void GetReviewDetailsAsync_InvalidId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await reviewService.GetReviewDetailsAsync(99));
        }

        [Test]
        public async Task CreateReviewAsync_AddsNewReview()
        {
            var model = new ReviewCreateDTO { GameId = 1, Content = "Excellent!", Rating = 5 };
            var userId = "user1";

            await reviewService.CreateReviewAsync(model, userId);
            var reviews = await context.Reviews.ToListAsync();

            Assert.AreEqual(1, reviews.Count);
            Assert.AreEqual("Excellent!", reviews.First().Content);
        }

        [Test]
        public async Task UpdateReviewAsync_ValidReview_UpdatesReview()
        {
            var review = new Review { ReviewId = 1, GameId = 1, UserId = "user1", Content = "Good", Rating = 3, CreatedAt = DateTime.UtcNow };
            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync();

            var updateDTO = new ReviewUpdateDTO {Id=1, GameId = 1, UserId = "user1", Content = "Updated Review", Rating = 5, CreatedAt = review.CreatedAt };

            await reviewService.UpdateReviewAsync(updateDTO);
            var updatedReview = await context.Reviews.FindAsync(1);

            Assert.AreEqual("Updated Review", updatedReview.Content);
            Assert.AreEqual(5, updatedReview.Rating);
        }

        [Test]
        public async Task UpdateReviewAsync_InvalidReview_ThrowsNotFoundException()
        { 
            var updateDTO = new ReviewUpdateDTO { Id = 1, GameId = 1, UserId = "user1", Content = "Updated Review", Rating = 5};

            Assert.ThrowsAsync<NotFoundException>(async () => await reviewService.UpdateReviewAsync(updateDTO));
        }

        [Test]
        public async Task DeleteReviewAsync_ValidId_DeletesReview()
        {
            var review = new Review { ReviewId = 1, GameId = 1, UserId = "user1", Content = "To be deleted", Rating = 2, CreatedAt = DateTime.UtcNow };
            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync();

            await reviewService.DeleteReviewAsync(1);
            var result = await context.Reviews.FindAsync(1);

            Assert.Null(result);
        }

        [Test]
        public void DeleteReviewAsync_InvalidId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await reviewService.DeleteReviewAsync(99));
        }
    }
}