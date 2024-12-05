namespace VenomGames.Core.DTOs.Review
{
    public class GetReviewsQuery
    {
        public int? GameId { get; set; }
        public string? UserId { get; set; }
        public decimal? Rating { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
