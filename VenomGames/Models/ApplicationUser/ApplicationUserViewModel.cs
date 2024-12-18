namespace VenomGames.Models.ApplicationUser
{
    public class ApplicationUserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
