using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Review;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    public class ReviewController : BaseController
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService _reviewService, IShoppingCartService _shoppingCartService, UserManager<ApplicationUser> _userManager)
            :base(_shoppingCartService,_userManager)
        {
            reviewService = _reviewService;
        }

        public async Task<IActionResult> Index(int id)
        {
            await SetCartItemCountAsync();

            var reviews = await reviewService.GetReviewsByGameIdAsync(id);

            if (!reviews.Any())
            {
                ViewBag.Message = "No reviews available for this game.";
            }

            return View(reviews);
        }

        public async Task<IActionResult> Details(int id)
        {
            ReviewOutputModel? review = await reviewService.GetReviewDetailsAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var model = new ReviewCreateDTO
            {
                GameId = id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized();
                }

                await reviewService.CreateReviewAsync(model, userId);
                return RedirectToAction("Index", "Review", new { id = model.GameId });
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ReviewOutputModel? review = await reviewService.GetReviewDetailsAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReviewUpdateDTO review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await reviewService.UpdateReviewAsync(review);
                return RedirectToAction(nameof(Index), new { gameId = review.GameId });
            }
            return View(review);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ReviewOutputModel? review = await reviewService.GetReviewDetailsAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ReviewOutputModel? review = await reviewService.GetReviewDetailsAsync(id);
            if (review != null)
            {
                await reviewService.DeleteReviewAsync(id);
            }
            return RedirectToAction(nameof(Index), new { gameId = review?.GameId });
        }
    }
}