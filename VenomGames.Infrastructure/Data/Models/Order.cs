using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static VenomGames.Infrastructure.Constants.DataConstants;
using static VenomGames.Infrastructure.Constants.ErrorMessages;

namespace VenomGames.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents an order placed by a user.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Unique order identifier.
        /// </summary>
        [Key]
        [Comment("Order identifier")]
        public int OrderId { get; set; }

        /// <summary>
        /// Unique identifier for the user who placed the order
        /// </summary>
        [Comment("Order user identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// User who placed the order.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        [Comment("Order user")]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Id of ordered game.
        /// </summary>
        [Comment("Order user")]
        public int GameId { get; set; }

        /// <summary>
        /// Ordered game
        /// </summary>
        [ForeignKey(nameof(GameId))]
        [Comment("Order game")]
        public Game Game { get; set; } = null!;

        /// <summary>
        /// Date when the order was placed.
        /// </summary>
        [Required]
        [Comment("Order date")]
        public DateTime OrderDate { get; set; }
    }
}
