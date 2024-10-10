using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static VenomGames.Infrastructure.Constants.DataConstants;
using static VenomGames.Infrastructure.Constants.ErrorMessages;

namespace VenomGames.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a review of a game.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Unique review identifier.
        /// </summary>
        [Key]
        [Comment("Review Identifier")]
        public int ReviewId { get; set; }

        /// <summary>
        /// Game identifier for associated game
        /// </summary>
        [Comment("Game Identifier")]
        public int GameId { get; set; }

        /// <summary>
        /// Game associated with the review
        /// </summary>
        [ForeignKey(nameof(GameId))]
        [Comment("Reviewed game")]
        public Game Game { get; set; } = null!;

        /// <summary>
        /// User identifier for reviewer.
        /// </summary>
        [Comment("User Identifier")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Content of the review.
        /// </summary>
        [Required]
        [StringLength(ReviewContentMaxLength, ErrorMessage = ReviewContentLengthError)]
        [Comment("Review content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Rating given by the user from 1-5.
        /// </summary>
        [Range(ReviewMinimumRating, ReviewMaximumRating)]
        public int Rating { get; set; }

        /// <summary>
        /// Date of review creation
        /// </summary>
        [Required]
        [Comment("Reveiw date")]
        public DateTime CreatedAt { get; set; }

    }
}
