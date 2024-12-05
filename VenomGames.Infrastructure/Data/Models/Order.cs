using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Comment("Order identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the user who placed the order
        /// </summary>
        [Comment("Order user identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// User who placed the order.
        /// </summary>
        [Comment("Order user")]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Total price of the order.
        /// </summary>
        [Comment("Order price")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Ordered games
        /// </summary>
        [Comment("Order games")]
        public ICollection<GameOrder> GameOrders { get; set; } = new List<GameOrder>();

        /// <summary>
        /// Date when the order was placed.
        /// </summary>
        [Comment("Order date")]
        public DateTime OrderDate { get; set; }
    }
}
