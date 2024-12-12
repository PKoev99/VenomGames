using VenomGames.Core.DTOs.CartItem;

namespace VenomGames.Core.DTOs.ShoppingCart
{
    public class ShoppingCartOutputModel
    {
        public int Id { get; set; }
        public List<CartItemOutputModel> Items { get; set; } = new List<CartItemOutputModel>();
        public decimal TotalPrice { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
