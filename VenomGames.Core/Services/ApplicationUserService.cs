using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VenomGames.Core.Common.Exceptions;
using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service for managing users.
    /// </summary>
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;

        public ApplicationUserService(UserManager<ApplicationUser> _userManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            context = _context;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            IEnumerable<ApplicationUser> users = await userManager.Users.ToListAsync();

            return users;
        }

        /// <summary>
        /// Retrieves a user by email.
        /// </summary>
        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(email);
            if (user == null) 
            {
                throw new NotFoundException(nameof(ApplicationUser),email);
            }
            
            return user;
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), id);
            }

            return user;
        }

        /// <summary>
        /// Registers a new user using ASP.NET Identity.
        /// </summary>
        public async Task<IdentityResult> RegisterUser(ApplicationUser user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Adds a user to a specified role.
        /// </summary>
        public async Task<IdentityResult> AddUserToRole(ApplicationUser user, string role)
        {
            return await userManager.AddToRoleAsync(user, role);
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        public async Task DeleteUserAsync(string id)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(id);
            if (user == null) 
            {
                throw new NotFoundException(nameof(ApplicationUser), id);
            }

            await userManager.DeleteAsync(user);
        }
    }
}
