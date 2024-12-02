using Microsoft.AspNetCore.Identity;
using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Services
{
    /// <summary>
    /// Service for managing users.
    /// </summary>
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<ApplicationUser> userRepository;

        public ApplicationUserService(UserManager<ApplicationUser> _userManager, IRepository<ApplicationUser> _userRepository)
        {
            userManager = _userManager;
            userRepository = _userRepository;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await userRepository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a user by email.
        /// </summary>
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await userManager.FindByIdAsync(id);
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
            await userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        public async Task DeleteUserAsync(int id)
        {
            await userRepository.DeleteAsync(id);
        }
    }
}
