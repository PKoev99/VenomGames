using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Infrastructure.Data.Configurations
{
    public class GameCategoryConfiguration : IEntityTypeConfiguration<GameCategory>
    {
        public void Configure(EntityTypeBuilder<GameCategory> builder)
        {
            builder.HasKey(go => new { go.CategoryId, go.GameId });

            builder.HasOne(gc => gc.Game)
                .WithMany(g => g.GameCategories)
                .HasForeignKey(go => go.CategoryId);

            builder.HasIndex(go => go.CategoryId);

            builder.HasOne(gc => gc.Game)
                .WithMany(g => g.GameCategories)
                .HasForeignKey(go => go.GameId);

            builder.HasIndex(go => go.GameId);
        }
    }
}
