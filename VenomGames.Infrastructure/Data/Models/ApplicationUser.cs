using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static VenomGames.Infrastructure.Constants.DataConstants;
using static VenomGames.Infrastructure.Constants.ErrorMessages;

namespace VenomGames.Infrastructure.Data.Models
{
    /// <summary>
    /// Extends ASP.NET Identity's base user class.
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        /// <summary>
        /// The user's collection of orders
        /// </summary>
        [Comment("User order collection")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
