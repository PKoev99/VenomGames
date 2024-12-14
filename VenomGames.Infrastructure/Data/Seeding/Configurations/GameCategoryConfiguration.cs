using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Infrastructure.Data.Seeding.Configurations
{
    public class GameCategoryConfiguration : IEntityTypeConfiguration<GameCategory>
    {
        public void Configure(EntityTypeBuilder<GameCategory> builder)
        {
            builder.HasKey(gc => new { gc.CategoryId, gc.GameId });

            builder.HasOne(gc => gc.Game)
                .WithMany(g => g.GameCategories)
                .HasForeignKey(gc => gc.GameId);

            builder.HasIndex(gc => gc.GameId);

            builder.HasOne(gc => gc.Category)
                .WithMany(c => c.GameCategories)
                .HasForeignKey(gc => gc.CategoryId);

            builder.HasIndex(gc => gc.CategoryId);
        }
    }
}
