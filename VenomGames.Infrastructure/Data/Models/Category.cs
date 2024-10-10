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
        [Key]
        [Comment("Category Identifier")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Name of the game category.
        /// </summary>
        [Required]
        [StringLength(CategoryNameMaxLength, ErrorMessage = CategoryNameLengthError)]
        [Comment("Category name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Collection of games associated with the category
        /// </summary>
        [Comment("Category game collection")]
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
