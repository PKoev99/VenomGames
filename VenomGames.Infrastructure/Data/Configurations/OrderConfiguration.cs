using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .UseIdentityColumn(1, 1);

            builder
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.UserId);

            builder
                .Property(o => o.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}
