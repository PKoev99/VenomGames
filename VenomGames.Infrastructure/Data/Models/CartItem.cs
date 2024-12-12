using VenomGames.Infrastructure.Data.Models;

public class CartItem
{
    /// <summary>
    /// Cart item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Shopping cart identifier
    /// </summary>
    public int ShoppingCartId { get; set; }

    /// <summary>
    /// Cart item shopping cart
    /// </summary>
    public ShoppingCart ShoppingCart { get; set; } = null!;

    /// <summary>
    /// Cart item game identifier.
    /// </summary>
    public int GameId { get; set; }

    /// <summary>
    /// Cart items game
    /// </summary>
    public Game Game { get; set; } = null!;

    /// <summary>
    /// Cart item quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Cart item price
    /// </summary>
    public decimal Price { get; set; }
}