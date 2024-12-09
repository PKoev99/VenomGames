using VenomGames.Models.Category;
using VenomGames.Models.Review;

namespace VenomGames.Models.Game
{
    public class GameViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<ReviewViewModel>? Reviews { get; set; }
        public List<CategoryViewModel>? Categories { get; set; }
    }
}
