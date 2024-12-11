using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Infrastructure.Data.Models;
using VenomGames.Models.ApplicationUser;

namespace VenomGames.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserService userService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUserController(IApplicationUserService _userService, SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager)
        {
            userService = _userService;
            signInManager = _signInManager;
            userManager = _userManager;
        }

        // GET: /Users
        public async Task<IActionResult> Index()
        {
            IEnumerable<ApplicationUser> users = await userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            TempData["Message"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = "/Home/Index")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = "/Home/Index")
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        // GET: /Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            ApplicationUser? user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: /Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser? user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await userService.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: /Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser? user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Users/ByEmail
        public async Task<IActionResult> GetByEmail(string email)
        {
            ApplicationUser? user = await userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}
