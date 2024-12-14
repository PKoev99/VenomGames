namespace VenomGames.Core.DTOs.Review
{
    public class ReviewOutputModel
    {
        public int ReviewId { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public decimal Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
