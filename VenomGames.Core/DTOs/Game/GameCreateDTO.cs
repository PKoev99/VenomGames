namespace VenomGames.Core.DTOs.Game
{
    public class GameCreateDTO
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
