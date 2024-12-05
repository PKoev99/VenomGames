using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VenomGames.Infrastructure.Data.Models;
using static VenomGames.Infrastructure.Constants.DataConstants;

namespace VenomGames.Infrastructure.Data.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .HasKey(g => g.Id);

            builder
                .Property(g => g.Id)
                .UseIdentityColumn(1, 1);

            builder
                .Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(GameTitleMaxLength)
                .HasColumnType($"nvarchar({GameTitleMaxLength})");

            builder
                .Property(g => g.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder
                .Property(g => g.Description)
                .IsRequired(false)
                .HasMaxLength(GameDescriptionMaxLength)
                .HasColumnType($"nvarchar({GameDescriptionMaxLength})");
        }
    }
}
