using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context of the application.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Game>()
                .HasOne(g => g.Category)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(o => o.Game)
                .WithMany(g => g.Orders)
                .HasForeignKey(o => o.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            builder.Entity<Game>()
                .HasKey(g => g.GameId);

            builder.Entity<Review>()
                .HasKey(r => r.ReviewId);

            builder.Entity<Order>()
                .HasKey(o => o.OrderId);

            builder.Entity<Game>()
                .Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
