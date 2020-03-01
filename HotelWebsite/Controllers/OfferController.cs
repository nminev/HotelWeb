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
            foreach (var item in _context.Offers)
            {
                result.Add(new OffersViewModel { ID = item.ID, Name = item.Name });
            }
            return View(result);
        }

        [Authorize(Roles = "Admin,Member")]
        public IActionResult Offer(int id)
        {

            var offer = _context.Offers.Single(x => x.ID == id);
            var result = new OfferViewModel
            {
                ID = offer.ID,
                Name = offer.Name,
                Description = offer.Description,
                Rating = offer.Rating,
                Price = offer.Price
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
            var newReview = new CreateReviewViewModel
            {
                OfferId = id,
                OfferName = offerName
            };

            return View(newReview);
        }

        [Authorize(Roles = "Admin,Member")]
        [HttpPost]
        public IActionResult CreateReview(CreateReviewViewModel review)
        {
            var reviewOffer = _context.Offers.Single(x => x.ID == review.OfferId);

            var user = _manager.GetUserAsync(HttpContext.User).Result;

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

            return RedirectToAction("Offers");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
