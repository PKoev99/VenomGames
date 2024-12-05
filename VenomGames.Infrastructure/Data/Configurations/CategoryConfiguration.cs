using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VenomGames.Infrastructure.Data.Models;
using static VenomGames.Infrastructure.Constants.DataConstants;

namespace VenomGames.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder
                .HasKey(c => c.CategoryId);

            builder.Property(c => c.CategoryId)
                .UseIdentityColumn(1,1);

            builder
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(CategoryNameMaxLength)
                .HasColumnType($"nvarchar({CategoryNameMaxLength})");
        }
    }
}
