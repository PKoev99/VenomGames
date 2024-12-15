namespace VenomGames.Core.DTOs.Game
{
    public class GameIndexOutputModel
    {
        public IEnumerable<GameOutputModel> Games { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchQuery { get; set; }
    }
}
