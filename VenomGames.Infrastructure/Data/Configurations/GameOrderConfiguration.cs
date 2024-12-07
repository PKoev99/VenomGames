using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Infrastructure.Data.Configurations
{
    public class GameOrderConfiguration : IEntityTypeConfiguration<GameOrder>
    {
        void IEntityTypeConfiguration<GameOrder>.Configure(EntityTypeBuilder<GameOrder> builder)
        {
            builder.HasKey(go => new { go.OrderId, go.GameId });

            builder.HasOne(go => go.Game)
                .WithMany(o => o.GameOrders)
                .HasForeignKey(go => go.GameId);

            builder.HasIndex(go => go.GameId);

            builder.HasOne(go => go.Order)
                .WithMany(g => g.GameOrders)
                .HasForeignKey(go => go.OrderId);

            builder.HasIndex(go => go.OrderId);
        }
    }
}
