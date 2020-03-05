using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HotelWebsite.Models;
using HotelWebsite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HotelWebsite.Controllers
{
    public class OfferController : Controller
    {
        private readonly ILogger<OfferController> _logger;
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _manager;

        public OfferController(ILogger<OfferController> logger, ApplicationDbContext context, UserManager<IdentityUser> manager)
        {
            _logger = logger;
            _context = context;
            _manager = manager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Member")]
        public IActionResult Offers()
        {
            var result = new List<OffersViewModel>();
            foreach (var item in _context.Offers.Where(x =>
            x.AvailableFrom <= DateTime.Now &&
            x.AvailableTo >= DateTime.Now &&
            x.User == null))
            {
                result.Add(new OffersViewModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Price = item.Price,
                    Raiting = item.Rating,
                    IsBooked = false
                });
            }
            return View(result);
        }

        [Authorize(Roles = "Admin,Member")]
        public IActionResult Offer(int id)
        {
            var offer = _context.Offers.Single(x => x.ID == id);
            var test = offer.UserId;
            var result = new OfferViewModel
            {
                ID = offer.ID,
                Name = offer.Name,
                Description = offer.Description,
                Raiting = offer.Rating,
                Price = offer.Price,
                IsBooked = offer.UserId != null
            };
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditOffer(int id)
        {
            var offer = _context.Offers.Single(x => x.ID == id);
            var result = new EditOfferViewModel
            {
                ID = offer.ID,
                Name = offer.Name,
                Description = offer.Description,
                Price = offer.Price
            };
            return View(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditOffer(EditOfferViewModel editOffer)
        {
            var offer = _context.Offers.Single(x => x.ID == editOffer.ID);

            offer.Description = editOffer.Description;
            offer.Name = editOffer.Name;
            offer.Price = editOffer.Price;

            _context.SaveChanges();
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateOffer()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateOffer(CreateOfferViewModel createOffer)
        {
            _context.Offers.Add(
                new Offer
                {
                    Name = createOffer.Name,
                    Price = createOffer.Price,
                    Description = createOffer.Description

                });

            _context.SaveChanges();

            return View("Index");
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

            return await Task.Run<ActionResult>(() => { return RedirectToAction("Offers"); }).ConfigureAwait(false);
        }

        public async Task<IActionResult> BookOffer(BookOffer bookOffer)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }
            var offer = _context.Offers.Single(x => x.ID == bookOffer.OfferId);    

            var user = await _manager.GetUserAsync(HttpContext.User).ConfigureAwait(false);

            offer.User = user;

            _context.SaveChanges();

            return await Task.Run<ActionResult>(() => { return RedirectToAction("Offer", "Offer", new { bookOffer.OfferId }); }).ConfigureAwait(false);
        }
        [Authorize(Roles = "Admin,Member")]
        public IActionResult GetBookedOffers(string userId)
        {
            var test = _context.Offers
                .Where(x => x.UserId == userId)
                .Select(x => new OffersViewModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    Price = x.Price,
                    Raiting = x.Rating,
                    IsBooked = true
                }).ToList();
            return View("Offers", test);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
