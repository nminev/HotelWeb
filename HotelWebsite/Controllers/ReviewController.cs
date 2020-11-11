using Database.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.ReviewModels;
using Service;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebsite.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ILogger<OfferController> _logger;
        private IReviewService _reviewService;
        private readonly UserManager<IdentityUser> _manager;


        public ReviewController(ILogger<OfferController> logger, IReviewService reviewService)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,Member")]
        [HttpGet]
        public IActionResult CreateReview(int id)
        {
            CreateReviewViewModel offerReview = _reviewService.GetReviewVM(id);

            return View(offerReview);
        }

        [Authorize(Roles = "Admin,Member")]
        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewViewModel review)
        {
            if (!this.ModelState.IsValid)
            {
                return View(review);
            }
            await _reviewService.AddReview(review, HttpContext.User).ConfigureAwait(false);

            return await Task.Run<ActionResult>(() => { return RedirectToAction("Offers", "Offer"); }).ConfigureAwait(false);
        }
    }
}
