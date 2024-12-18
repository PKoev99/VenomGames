namespace VenomGames.Core.DTOs.Order
{
    public class OrderCheckoutDTO
    {
        public ICollection<OrderItemDTO> CartItems { get; set; } = new List<OrderItemDTO>();
        public decimal TotalPrice { get; set; }
    }
}
