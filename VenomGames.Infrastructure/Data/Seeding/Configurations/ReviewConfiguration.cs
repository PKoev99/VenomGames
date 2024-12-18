using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VenomGames.Infrastructure.Data.Models;
using static VenomGames.Infrastructure.Constants.DataConstants;

namespace VenomGames.Infrastructure.Data.Seeding.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
                .HasKey(r => r.ReviewId);

            builder.Property(c => c.ReviewId)
                .UseIdentityColumn(1, 1);

            builder
                .HasOne(r => r.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => r.GameId);

            builder
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => r.UserId);

            builder
                .Property(r => r.Content)
                .IsRequired(false)
                .HasMaxLength(ReviewContentMaxLength)
                .HasColumnType($"nvarchar({ReviewContentMaxLength})");

            builder
                .Property(r => r.Rating)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}
