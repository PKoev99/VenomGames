using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Infrastructure.Data.Seeding
{
    public class DataSeeder
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;
        
        public DataSeeder(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            context = _context;
        }

        public async Task SeedRolesAsync()
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }

        public async Task SeedAdminAsync()
        {
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com"
                };

                var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        public async Task SeedCategoriesAsync()
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
            {
                new Category { Name = "Action" },
                new Category { Name = "RPG" },
                new Category { Name = "Adventure" },
                new Category { Name = "Shooter" },
                new Category { Name = "Strategy" },
                new Category { Name = "Simulation" },
                new Category { Name = "Sports" },
                new Category { Name = "Horror" },
                new Category { Name = "Indie" },
                new Category { Name = "Multiplayer" },
                new Category { Name = "MMO" },
                new Category { Name = "Sandbox" },
                new Category { Name = "Puzzle" },
                new Category { Name = "Survival" },
                new Category { Name = "Open World" },
                new Category { Name = "Singleplayer" },
                new Category { Name = "Fighting" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }

        public async Task SeedGamesAsync()
        {
            if (!context.Games.Any())
            {
                var games = new List<Game>
            {
                new Game { Title = "The Witcher 3: Wild Hunt", Price = 49.99M, Description = "A story-driven open-world RPG.", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS3FCb7yXwsvBDtbAyTS0dH2VDPpJe7RZd8ntz60Wu_HPmv021zZ857Fjgxu1VIOWwIOxMI-g" },
                new Game { Title = "Red Dead Redemption 2", Price = 59.99M, Description = "An epic tale of life in America’s unforgiving heartland.", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRQyXi1Jjdtp7_Pqp7_lK7OjGc5x2pwcD70FadC9sCmbzx4wRisjwYW944IXXHTwqSL16Ru" },
                new Game { Title = "Cyberpunk 2077", Price = 59.99M, Description = "A futuristic open-world RPG set in a dystopian world.", ImageUrl = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcTBOHvidaV_97rHzZIUi6IyEVjNEFPvsUMWsXftbmrhFYgr_CoSSzbgmfg-YnqOjhsjwlLN" },
                new Game { Title = "Minecraft", Price = 26.99M, Description = "A sandbox game where you can build and explore a procedurally generated world.", ImageUrl = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcRCfrlrqsrk5bLN5agxC3CFMnEJlavmRD4UoagaDNUktXH3U8TdeHOFC2U7D3FsCj9q8IYq9g" },
                new Game { Title = "Call of Duty: Modern Warfare", Price = 59.99M, Description = "A first-person shooter with an intense multiplayer mode.", ImageUrl = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcRrPOjUQS_qoV5_5F6d6k4CR036_mRFvy3J8dQu8fC1mu-PtcRth4wyaq2qDTFnOKyoAxaW" },
                new Game { Title = "Battlefield 2042", Price = 69.99M, Description = "A large-scale warfare FPS set in a near-future world.", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQz7dcRkgqgQV1ybm5TcMCbYF5fbDkVcHB3OViMUnbuhc5olWDuJNmX-GycM0fbYWrRZzC0" },
                new Game { Title = "Assassin's Creed Odyssey", Price = 39.99M, Description = "An open-world RPG set in ancient Greece with historical narratives.", ImageUrl = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcSXs8AAPLanXzyRdx2_aDKIo0ThVN2J8XrJpDzb7TFxC3DNi87LOKPFTt9ChSCJXwFBTUkM" },
                new Game { Title = "The Elder Scrolls V: Skyrim", Price = 39.99M, Description = "An open-world RPG where you fight dragons and explore a vast world.", ImageUrl = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcSkYHqTq8Qt5n5q1ohLQWdfuYhMsBoIPKogQ509BF_HK3KF70DkhApnr9uBG8NDPQxLtXrNTw" },
                new Game { Title = "Grand Theft Auto V", Price = 29.99M, Description = "A third-person open-world action-adventure game.", ImageUrl = "https://media-rockstargames-com.akamaized.net/rockstargames-newsite/img/global/games/fob/640/V.jpg" },
                new Game { Title = "Red Dead Redemption", Price = 39.99M, Description = "A western action-adventure game set in the early 20th century.", ImageUrl = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSwUHK5x2GwuhnziIx7ELGzmkjMqgIc1lz0CFjTJclrt1D-Glu1n5RdBuoh0-F7fBD4kBQy" },
                new Game { Title = "Far Cry 5", Price = 39.99M, Description = "An open-world first-person shooter with a focus on freedom and exploration.", ImageUrl = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcTWD84MJJtjSRwj3BJW5hS-mItNG4MuQ51IuYep_66hZhcLhTdMjc67X0v-1ywnNsC_UC0yXw" },
                new Game { Title = "Dark Souls III", Price = 59.99M, Description = "A punishingly difficult action RPG with deep lore and world-building.", ImageUrl = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcRaGkCHLYs81irO2QiWmd5rGmrJdITo5kBqXNJGc2AsifZoQ3OZ2ISIaPTjhKdrQWrOOqMe" },
                new Game { Title = "The Legend of Zelda: Breath of the Wild", Price = 59.99M, Description = "An open-world action-adventure game in the Zelda universe.", ImageUrl = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcSOf5S2naez6ebfhbGJAqV_hsisIMkxjO49FQYyZTzkGqPkhiuFIBGbUrkoxSncb6P1ZTdkQg" },
                new Game { Title = "DOOM Eternal", Price = 59.99M, Description = "A fast-paced, action-packed first-person shooter.", ImageUrl = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSwXpA27qaSPqshhkam0bstRUskakflxld7rP6C7cHCBMkrW3vMmSYpFzr0xvXz5n_ydYV2" },
                new Game { Title = "FIFA 23", Price = 69.99M, Description = "A football simulation video game featuring realistic graphics and gameplay.", ImageUrl = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcSi2n-rkmzwypmEnSd6qI0DeZgUJG8xhB19gQDlxgrVEq6JEdnJtbZ4xI8JBmCv0mMK1L8Fug" },
                new Game { Title = "NBA 2K23", Price = 59.99M, Description = "A basketball simulation video game with realistic gameplay.", ImageUrl = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcS_kuKEMVIW7TzVSbLIkjYuyjTnPXizkfUxm6ZC8V37h2Lv1dpEsSQHuZmrKjTdICqmDUZE" },
                new Game { Title = "The Sims 4", Price = 39.99M, Description = "A life simulation game where players control the lives of their virtual characters.", ImageUrl = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcQkWyk7fsr9sOe7Z6A8AOGPMf8KYQ7nPB-4gt9j_jzgLTN9k05eF705q-CKwRCZ9BnlSDzF" },
                new Game { Title = "Hitman 3", Price = 59.99M, Description = "A stealth-based action game where you play as a hitman on assignments worldwide.", ImageUrl = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQv-4K6HMuTLI-4mdtN716h9g0X9_cyktcH08ltcJhIuc92ftOOlMur36vy_G1ZZv7a4TJ2" },
                new Game { Title = "Tomb Raider (2013)", Price = 19.99M, Description = "A reimagining of the iconic action-adventure series featuring Lara Croft.", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRYhE6Ro7kY0as4njZevpeQk8G47EhRC3kRR0HoTzuxVJj7p13CwFFdUmlZHtm6bBOsSGt5" },
                new Game { Title = "No Man's Sky", Price = 59.99M, Description = "A survival game set in a procedurally generated universe with crafting and exploration.", ImageUrl = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcQLR9J84Pl_x_satd6wgQJNXPP0QngGQBSzfGYhGyVUbK6eR0x0M1FLwCB2tX0ZSQQqx3kk" }
                };

                context.Games.AddRange(games);
                await context.SaveChangesAsync();
            }
        }

        public async Task SeedGameCategoriesAsync()
        {
            if (!context.GameCategories.Any())
            {
                var actionCategory = context.Categories.First(c => c.Name == "Action");
                var rpgCategory = context.Categories.First(c => c.Name == "RPG");
                var adventureCategory = context.Categories.First(c => c.Name == "Adventure");
                var shooterCategory = context.Categories.First(c => c.Name == "Shooter");
                var strategyCategory = context.Categories.First(c => c.Name == "Strategy");
                var simulationCategory = context.Categories.First(c => c.Name == "Simulation");
                var sportsCategory = context.Categories.First(c => c.Name == "Sports");
                var horrorCategory = context.Categories.First(c => c.Name == "Horror");
                var indieCategory = context.Categories.First(c => c.Name == "Indie");
                var multiplayerCategory = context.Categories.First(c => c.Name == "Multiplayer");
                var mmoCategory = context.Categories.First(c => c.Name == "MMO");
                var sandboxCategory = context.Categories.First(c => c.Name == "Sandbox");
                var puzzleCategory = context.Categories.First(c => c.Name == "Puzzle");
                var survivalCategory = context.Categories.First(c => c.Name == "Survival");
                var fightingCategory = context.Categories.First(c => c.Name == "Fighting");
                var openWorldCategory = context.Categories.First(c => c.Name == "Open World");
                var singleplayerCategory = context.Categories.First(c => c.Name == "Singleplayer");

                var game1 = context.Games.First(g => g.Title == "The Witcher 3: Wild Hunt");
                var game2 = context.Games.First(g => g.Title == "Red Dead Redemption 2");
                var game3 = context.Games.First(g => g.Title == "Cyberpunk 2077");
                var game4 = context.Games.First(g => g.Title == "Minecraft");
                var game5 = context.Games.First(g => g.Title == "Call of Duty: Modern Warfare");
                var game6 = context.Games.First(g => g.Title == "Battlefield 2042");
                var game7 = context.Games.First(g => g.Title == "Assassin's Creed Odyssey");
                var game8 = context.Games.First(g => g.Title == "The Elder Scrolls V: Skyrim");
                var game9 = context.Games.First(g => g.Title == "Grand Theft Auto V");
                var game10 = context.Games.First(g => g.Title == "Far Cry 5");
                var game11 = context.Games.First(g => g.Title == "Dark Souls III");
                var game12 = context.Games.First(g => g.Title == "The Legend of Zelda: Breath of the Wild");
                var game13 = context.Games.First(g => g.Title == "DOOM Eternal");
                var game14 = context.Games.First(g => g.Title == "FIFA 23");
                var game15 = context.Games.First(g => g.Title == "NBA 2K23");
                var game16 = context.Games.First(g => g.Title == "The Sims 4");
                var game17 = context.Games.First(g => g.Title == "Hitman 3");
                var game18 = context.Games.First(g => g.Title == "Tomb Raider (2013)");
                var game19 = context.Games.First(g => g.Title == "No Man's Sky");

                context.GameCategories.AddRange(
                    new GameCategory { GameId = game1.Id, CategoryId = actionCategory.CategoryId },
                    new GameCategory { GameId = game1.Id, CategoryId = openWorldCategory.CategoryId },
                    new GameCategory { GameId = game1.Id, CategoryId = rpgCategory.CategoryId },
                    new GameCategory { GameId = game2.Id, CategoryId = actionCategory.CategoryId },
                    new GameCategory { GameId = game2.Id, CategoryId = adventureCategory.CategoryId },
                    new GameCategory { GameId = game2.Id, CategoryId = rpgCategory.CategoryId },
                    new GameCategory { GameId = game3.Id, CategoryId = rpgCategory.CategoryId },
                    new GameCategory { GameId = game3.Id, CategoryId = openWorldCategory.CategoryId },
                    new GameCategory { GameId = game4.Id, CategoryId = sandboxCategory.CategoryId },
                    new GameCategory { GameId = game4.Id, CategoryId = survivalCategory.CategoryId },
                    new GameCategory { GameId = game5.Id, CategoryId = shooterCategory.CategoryId },
                    new GameCategory { GameId = game5.Id, CategoryId = singleplayerCategory.CategoryId },
                    new GameCategory { GameId = game5.Id, CategoryId = multiplayerCategory.CategoryId },
                    new GameCategory { GameId = game6.Id, CategoryId = shooterCategory.CategoryId },
                    new GameCategory { GameId = game6.Id, CategoryId = multiplayerCategory.CategoryId },
                    new GameCategory { GameId = game7.Id, CategoryId = openWorldCategory.CategoryId },
                    new GameCategory { GameId = game7.Id, CategoryId = rpgCategory.CategoryId },
                    new GameCategory { GameId = game7.Id, CategoryId = singleplayerCategory.CategoryId },
                    new GameCategory { GameId = game8.Id, CategoryId = rpgCategory.CategoryId },
                    new GameCategory { GameId = game8.Id, CategoryId = openWorldCategory.CategoryId },
                    new GameCategory { GameId = game8.Id, CategoryId = singleplayerCategory.CategoryId },
                    new GameCategory { GameId = game8.Id, CategoryId = adventureCategory.CategoryId },
                    new GameCategory { GameId = game9.Id, CategoryId = actionCategory.CategoryId },
                    new GameCategory { GameId = game9.Id, CategoryId = multiplayerCategory.CategoryId },
                    new GameCategory { GameId = game9.Id, CategoryId = singleplayerCategory.CategoryId },
                    new GameCategory { GameId = game9.Id, CategoryId = openWorldCategory.CategoryId },
                    new GameCategory { GameId = game10.Id, CategoryId = actionCategory.CategoryId },
                    new GameCategory { GameId = game10.Id, CategoryId = adventureCategory.CategoryId },
                    new GameCategory { GameId = game10.Id, CategoryId = openWorldCategory.CategoryId },
                    new GameCategory { GameId = game10.Id, CategoryId = multiplayerCategory.CategoryId },
                    new GameCategory { GameId = game11.Id, CategoryId = rpgCategory.CategoryId },
                    new GameCategory { GameId = game12.Id, CategoryId = actionCategory.CategoryId },
                    new GameCategory { GameId = game12.Id, CategoryId = adventureCategory.CategoryId },
                    new GameCategory { GameId = game13.Id, CategoryId = shooterCategory.CategoryId },
                    new GameCategory { GameId = game13.Id, CategoryId = actionCategory.CategoryId },
                    new GameCategory { GameId = game14.Id, CategoryId = sportsCategory.CategoryId },
                    new GameCategory { GameId = game15.Id, CategoryId = sportsCategory.CategoryId },
                    new GameCategory { GameId = game15.Id, CategoryId = simulationCategory.CategoryId },
                    new GameCategory { GameId = game16.Id, CategoryId = simulationCategory.CategoryId },
                    new GameCategory { GameId = game17.Id, CategoryId = actionCategory.CategoryId },
                    new GameCategory { GameId = game17.Id, CategoryId = adventureCategory.CategoryId },
                    new GameCategory { GameId = game18.Id, CategoryId = actionCategory.CategoryId },
                    new GameCategory { GameId = game18.Id, CategoryId = adventureCategory.CategoryId },
                    new GameCategory { GameId = game18.Id, CategoryId = puzzleCategory.CategoryId },
                    new GameCategory { GameId = game18.Id, CategoryId = shooterCategory.CategoryId },
                    new GameCategory { GameId = game19.Id, CategoryId = survivalCategory.CategoryId },
                    new GameCategory { GameId = game19.Id, CategoryId = actionCategory.CategoryId },
                    new GameCategory { GameId = game19.Id, CategoryId = adventureCategory.CategoryId },
                    new GameCategory { GameId = game19.Id, CategoryId = shooterCategory.CategoryId }
                );

                await context.SaveChangesAsync();
            }
        }

            public async Task SeedAllAsync()
        {
            await SeedCategoriesAsync();
            await SeedGamesAsync();
            await SeedGameCategoriesAsync();
        }
    }
}
