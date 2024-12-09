namespace VenomGames.Models.Order
{
    public class OrderItemViewModel
    {
        public int GameId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
