namespace VenomGames.Core.DTOs.Review
{
    public class ReviewOutputModel
    {
        public int ReviewId { get; set; }

        public int GameId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string? Content { get; set; } = string.Empty;

        public decimal Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
