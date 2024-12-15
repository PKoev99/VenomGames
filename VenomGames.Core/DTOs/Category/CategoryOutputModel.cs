using System.ComponentModel.DataAnnotations;
using static VenomGames.Infrastructure.Constants.DataConstants;
using static VenomGames.Infrastructure.Constants.ErrorMessages;

namespace VenomGames.Core.DTOs.Category
{
    public class CategoryOutputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength, ErrorMessage = CategoryNameLengthError)]
        public string Name { get; set; }
        public ICollection<Infrastructure.Data.Models.GameCategory> Games { get; set; } = null!;
    }
}
