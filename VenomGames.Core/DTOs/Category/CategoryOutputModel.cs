namespace VenomGames.Core.DTOs.Category
{
    public class CategoryOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Infrastructure.Data.Models.GameCategory> Games { get; set; } = null!;
    }
}
