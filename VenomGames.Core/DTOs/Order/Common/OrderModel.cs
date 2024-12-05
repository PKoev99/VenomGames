using Microsoft.Identity.Client;

namespace VenomGames.Core.DTOs.Order.Common
{
    public abstract class OrderModel
    {
        public string UserId { get; set; } = null!;
        public decimal Price { get; set; }
        public int GameId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
