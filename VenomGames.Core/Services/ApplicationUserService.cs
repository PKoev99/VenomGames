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
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return userRepository.GetAll();
        }

        /// <summary>
        /// Retrieves a user by email.
        /// </summary>
        public ApplicationUser GetUserByEmail(string email)
        {
            return userRepository.GetAll().FirstOrDefault(u => u.Email == email);
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        public ApplicationUser GetUserById(int id)
        {
            return userRepository.GetById(id);
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
        public void UpdateUser(ApplicationUser user)
        {
            userRepository.Update(user);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        public void DeleteUser(int id)
        {
            userRepository.Delete(id);
        }
    }
}
