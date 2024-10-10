using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static VenomGames.Infrastructure.Constants.DataConstants;
using static VenomGames.Infrastructure.Constants.ErrorMessages;

namespace VenomGames.Infrastructure.Data.Models
{
    /// <summary>
    /// Unique game identifier.
    /// </summary>
    public class Game
    {
        [Key]
        [Comment("Game Identifier")]
        public int GameId { get; set; }

        /// <summary>
        /// Title of the game.
        /// </summary>
        [Required]
        [StringLength(GameTitleMaxLength, ErrorMessage = GameTitleLengthError)]
        [Comment("Game title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Price of the game.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), GameMinimumPrice, GameMaximumPrice, ErrorMessage = GamePriceError)]
        [Comment("Game price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Short description of the game.
        /// </summary>
        [StringLength(GameDescriptionMaxLength, ErrorMessage = GameDescriptionLengthError)]
        [Comment("Game description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Category Identifier for the game
        /// </summary>
        [Comment("Game category Identifier")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Category the game belongs to.
        /// </summary>
        [ForeignKey(nameof(CategoryId))]
        [Comment("Game category")]
        public Category Category { get; set; } = null!;

        /// <summary>
        /// Game review collection
        /// </summary>
        [Comment("Game review collection")]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
