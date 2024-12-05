using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static VenomGames.Infrastructure.Constants.DataConstants;
using static VenomGames.Infrastructure.Constants.ErrorMessages;

namespace VenomGames.Infrastructure.Data.Models
{
    /// <summary>
    /// Unique game identifier.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Unique game Identifier.
        /// </summary>
        [Comment("Game Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Title of the game.
        /// </summary>
        [Comment("Game title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Price of the game.
        /// </summary>
        [Comment("Game price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Short description of the game.
        /// </summary>
        [Comment("Game description")]
        public string? Description { get; set; }

        /// <summary>
        /// Category collectiong for the game.
        /// </summary>        
        [Comment("Game category")]
        public ICollection<GameCategory> GameCategories { get; set; } = new List<GameCategory>();

        /// <summary>
        /// Game review collection
        /// </summary>
        [Comment("Game review collection")]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        /// <summary>
        /// Game order collection
        /// </summary>
        [Comment("Game review collection")]
        public ICollection<GameOrder> GameOrders { get; set; } = new List<GameOrder>();
    }
}
