using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Contracts;
using VenomGames.Core.Services;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;
using VenomGames.Infrastructure.Data.Seeding;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<DataSeeder>();
            services.AddHttpContextAccessor();

            return services;
        }
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
