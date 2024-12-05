namespace VenomGames.Core.DTOs.Game
{
    public class GameOutputModel
    {
        public int GameId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<Infrastructure.Data.Models.GameCategory> GameCategories { get; set; } = null!;
        public ICollection<Infrastructure.Data.Models.Review> Reviews { get; set; } = null!;
    }
}
