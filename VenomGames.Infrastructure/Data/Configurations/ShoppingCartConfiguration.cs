using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Infrastructure.Data.Configurations
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(o => o.Id)
                .UseIdentityColumn(1, 1);

            builder
                .HasMany(c => c.Items)
                .WithOne(c => c.ShoppingCart)
                .HasForeignKey(c => c.ShoppingCartId);

            builder
                .HasOne(sc => sc.User)
                .WithMany()
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasIndex(sc => sc.UserId);

            builder
                .Property(sc => sc.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}
