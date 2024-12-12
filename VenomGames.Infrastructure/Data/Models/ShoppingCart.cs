using VenomGames.Infrastructure.Data.Models;

public class ShoppingCart
{
    /// <summary>
    /// Shopping cart identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Shopping carts user identifier.
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Shopping cart user
    /// </summary>
    public ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// Shopping cart game collection
    /// </summary>
    public ICollection<CartItem> Items { get; set; } = null!;

    /// <summary>
    /// Shopping cart total price
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Shopping cart creation date
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Shopping carts order date
    /// </summary>
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// Shopping cart completion check
    /// </summary>
    public bool IsCompleted { get; set; }

}
