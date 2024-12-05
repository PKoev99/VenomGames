using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static VenomGames.Infrastructure.Constants.DataConstants;
using static VenomGames.Infrastructure.Constants.ErrorMessages;

namespace VenomGames.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a category for games.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Unique category identifier.
        /// </summary>
        [Comment("Category Identifier")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Name of the game category.
        /// </summary>
        [Comment("Category name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Collection of games associated with the category
        /// </summary>
        [Comment("Category game collection")]
        public ICollection<GameCategory> GameCategories { get; set; } = new List<GameCategory>();
    }
}
