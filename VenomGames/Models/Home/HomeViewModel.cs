using VenomGames.Core.DTOs.Category;
using VenomGames.Core.DTOs.Game;
using VenomGames.Models.Category;
using VenomGames.Models.Game;

namespace VenomGames.Models.Home
{
    public class HomeViewModel
    {
        public string? CategoryName { get; set; }
        public IEnumerable<GameOutputModel> FeaturedGames { get; set; } = new List<GameOutputModel>();
        public IEnumerable<CategoryOutputModel> Categories { get; set; } = new List<CategoryOutputModel>();
        public string WelcomeMessage { get; set; } = "Welcome to Venom Games!";
    }
}
