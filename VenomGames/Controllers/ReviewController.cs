using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> ByGame(int gameId)
        {
            IEnumerable<ReviewOutputModel> reviews = await reviewService.GetReviewsByGameIdAsync(gameId);
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

        // GET: /Reviews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewCreateDTO review)
        {
            if (ModelState.IsValid)
            {
                await reviewService.CreateReviewAsync(review);
                return RedirectToAction(nameof(ByGame), new { gameId = review.GameId });
            }
            return View(review);
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
                return RedirectToAction(nameof(ByGame), new { gameId = review.GameId });
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
            return RedirectToAction(nameof(ByGame), new { gameId = review?.GameId });
        }
    }
}
