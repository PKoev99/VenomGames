namespace VenomGames.Core.DTOs.Order
{
    public class GetOrdersQuery
    {
        public string? UserId { get; set; }
        public int? GameId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
