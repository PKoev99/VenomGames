using VenomGames.Core.DTOs.Review;

namespace VenomGames.Core.DTOs.Game
{
    public class GameOutputModel
    {
        public int GameId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public decimal AverageRating { get; set; }
        public ICollection<Infrastructure.Data.Models.GameCategory> GameCategories { get; set; } = null!;
        public IEnumerable<int> SelectedCategoryIds { get; set; } = new List<int>();
        public ICollection<ReviewOutputModel> Reviews { get; set; } = new List<ReviewOutputModel>();
    }
}