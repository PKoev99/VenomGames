using Microsoft.EntityFrameworkCore;

namespace VenomGames.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents Mapping table between Game and Order
    /// </summary>
    public class GameOrder
    {
        /// <summary>
        /// Game identifier
        /// </summary>
        [Comment("Game Id")]
        public int GameId { get; set; }

        /// <summary>
        /// Game entity
        /// </summary>
        [Comment("Game entity")]
        public Game Game { get; set; } = null!;

        /// <summary>
        /// Order identfier
        /// </summary>
        [Comment("Order Id")]
        public int OrderId { get; set; }

        /// <summary>
        /// Order entity
        /// </summary>
        [Comment("Order entity")]
        public Order Order { get; set; } = null!;

        /// <summary>
        /// Quantity of ordered items
        /// </summary>
        public int Quantity { get; set; }
    }
}
