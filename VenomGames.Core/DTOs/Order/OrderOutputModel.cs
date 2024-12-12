namespace VenomGames.Core.DTOs.Order
{
    public class OrderOutputModel
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public ICollection<Infrastructure.Data.Models.GameOrder> GameOrders { get; set; } =  null!;
    }
}