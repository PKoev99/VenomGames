using Microsoft.EntityFrameworkCore;

namespace VenomGames.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents Mapping table between Game and Category
    /// </summary>
    public class GameCategory
    {
        /// <summary>
        /// Game identifier
        /// </summary>
        [Comment("Game Id")]
        public int GameId { get; set; }

        /// <summary>
        /// Game entity
        /// </summary>
        [Comment("Game Entity")]
        public Game Game { get; set; } = null!;

        /// <summary>
        /// Category identifier
        /// </summary>
        [Comment("Category Id")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Category entity
        /// </summary>
        [Comment("Category entity")]
        public Category Category { get; set; } = null!;
    }
}
