using System.ComponentModel.DataAnnotations;
using static VenomGames.Infrastructure.Constants.DataConstants;

namespace VenomGames.Models.ApplicationUser
{
    public class ApplicationUserEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ApplicationUserNameMaxLength)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? Role { get; set; }

        public IEnumerable<string> AvailableRoles { get; set; } = new List<string>();
    }
}
