using System.ComponentModel.DataAnnotations;
using static VenomGames.Infrastructure.Constants.DataConstants;
using static VenomGames.Infrastructure.Constants.ErrorMessages;

namespace VenomGames.Core.DTOs.Category.Common
{
    public abstract class CategoryModel
    {
        [Required]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength, ErrorMessage = CategoryNameLengthError)]
        public string Name { get; set; } 
    }
}
