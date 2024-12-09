namespace VenomGames.Models.Review
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
