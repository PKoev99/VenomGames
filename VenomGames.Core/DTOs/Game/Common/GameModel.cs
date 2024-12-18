using System.ComponentModel.DataAnnotations;

namespace VenomGames.Core.DTOs.Game.Common
{
    public abstract class GameModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select at least one category.")]
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();

    }
}
