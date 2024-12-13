using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Review;

namespace VenomGames.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService _reviewService)
        {
            reviewService = _reviewService;
        }

        // GET: /Reviews/ByGame/5
        public async Task<IActionResult> Index(int id)
        {
            var reviews = await reviewService.GetReviewsByGameIdAsync(id);

            if (!reviews.Any())
            {
                ViewBag.Message = "No reviews available for this game.";
            }

            return View(reviews);
        }

        // GET: /Reviews/Details/5
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
                GameId = id // Automatically link this review to the game
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                // Get the current user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    // Handle case where no user is logged in
                    return Unauthorized();
                }

                // Pass the userId to the service
                await reviewService.CreateReviewAsync(model, userId);
                return RedirectToAction("Index", "Review", new { id = model.GameId });
            }

            return View(model);
        }

        // GET: /Reviews/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ReviewOutputModel? review = await reviewService.GetReviewDetailsAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: /Reviews/Edit/5
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

        // GET: /Reviews/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            ReviewOutputModel? review = await reviewService.GetReviewDetailsAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: /Reviews/Delete/5
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
