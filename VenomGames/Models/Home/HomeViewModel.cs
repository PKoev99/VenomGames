using VenomGames.Models.Category;
using VenomGames.Models.Game;

namespace VenomGames.Models.Home
{
    public class HomeViewModel
    {
        public IEnumerable<GameViewModel> FeaturedGames { get; set; } = new List<GameViewModel>();
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public string WelcomeMessage { get; set; } = "Welcome to Venom Games!";
    }
}
