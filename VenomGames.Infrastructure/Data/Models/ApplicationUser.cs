using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace VenomGames.Infrastructure.Data.Models
{
    /// <summary>
    /// Extends ASP.NET Identity's base user class.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The user's collection of orders
        /// </summary>
        [Comment("User order collection")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// The user's collection of reviews
        /// </summary>
        [Comment("User order collection")]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        /// <summary>
        /// The user's collection of games
        /// </summary>
        [Comment("User order collection")]
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}