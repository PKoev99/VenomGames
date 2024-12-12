using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VenomGames.Infrastructure.Data.Configurations
{
    internal class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(o => o.Id)
                .UseIdentityColumn(1, 1);

            builder
                .HasOne(ci => ci.ShoppingCart)
                .WithMany(sc => sc.Items)
                .HasForeignKey(ci => ci.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ShoppingCartId);

            builder
                .HasOne(ci => ci.Game)
                .WithMany()
                .HasForeignKey(ci => ci.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.GameId);

            builder
                .Property(ci=>ci.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
