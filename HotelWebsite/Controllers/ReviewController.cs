using Database.Data;
using HotelWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebsite.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ILogger<OfferController> _logger;
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _manager;


        public ReviewController(ILogger<OfferController> logger, UserManager<IdentityUser> manager, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _manager = manager;
        }

        [Authorize(Roles = "Admin,Member")]
        [HttpGet]
        public IActionResult CreateReview(int id)
        {
            var offerName = _context.Offers.Single(x => x.ID == id).Name;
            var reviews = _context.Reviews.Where(x => x.Offer.ID == id)?.ToList();
            var offerReview = new CreateReviewViewModel
            {
                OfferId = id,
                OfferName = offerName,
                Reviews = reviews
            };

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
            var reviewOffer = _context.Offers.Single(x => x.ID == review.OfferId);

            var user = await _manager.GetUserAsync(HttpContext.User).ConfigureAwait(false);

            var OfferReviews = _context.Reviews.Where(x => x.Offer.ID == review.OfferId).ToList();

            if (OfferReviews.Any())
            {
                var sum = OfferReviews.Sum(x => (double)x.Score) + review.Score;
                var count = OfferReviews.Count + 1;
                reviewOffer.Rating = sum / count;
            }
            else
            {
                reviewOffer.Rating = review.Score;
            }

            _context.Reviews.Add(
                 new Review
                 {
                     Title = review.Title,
                     Comment = review.Comment,
                     Score = review.Score,
                     Offer = reviewOffer,
                     Reviewer = user
                 });


            _context.SaveChanges();

            return await Task.Run<ActionResult>(() => { return RedirectToAction("Offers","Offer"); }).ConfigureAwait(false);
        }
    }
}
