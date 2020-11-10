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
using System;
using Microsoft.AspNetCore.Hosting;

namespace HotelWebsite.Controllers
{
    public class OfferController : Controller
    {
        private readonly ILogger<OfferController> _logger;
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _manager;
        private readonly IWebHostEnvironment _env;

        public OfferController(ILogger<OfferController> logger, ApplicationDbContext context, UserManager<IdentityUser> manager, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _manager = manager;
            _env = env;
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
            x.User == null))
            {
                var imageSrc = _context.OfferImages.FirstOrDefault(x => x.OfferId == item.ID)?.ImagePath;

                result.Add(new OffersViewModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Price = item.Price,
                    Raiting = item.Rating,
                    IsBooked = false,
                    ImageSrc = imageSrc
                });
            }
            return View(result);
        }

        [Authorize(Roles = "Admin,Member")]
        [HttpGet]
        public IActionResult Offer(int id)
        {
            OfferViewModel result = GetOffer(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Offer(OfferViewModel bookOffer)
        {
            if (!this.ModelState.IsValid)
            {
                if (bookOffer.From < bookOffer.To)
                {
                    ModelState.AddModelError(string.Empty, "The \"To\" date cannot be smaller then the \"From\" date");
                }
                bookOffer = GetOffer(bookOffer.ID);
                return View(bookOffer);
            }
            var offer = _context.Offers.Single(x => x.ID == bookOffer.ID);

            var user = await _manager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            if (user == null)
            {
                return BadRequest("User is null");
            }
            offer.User = user;
            offer.AvailableFrom = bookOffer.From;
            offer.AvailableTo = bookOffer.To;
            _context.SaveChanges();
            return await RedirectToActionAwait(bookOffer);
        }

        private async Task<IActionResult> RedirectToActionAwait(OfferViewModel bookOffer)
        {
            return await Task.Run<ActionResult>(() => { return RedirectToAction("Offer", "Offer", new { id = bookOffer.ID }); }).ConfigureAwait(false);
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
            string imagePath = string.Empty;

            offer.Description = editOffer.Description;
            offer.Name = editOffer.Name;
            offer.Price = editOffer.Price;
            if (!string.IsNullOrWhiteSpace(editOffer.ImageName))
            {
                string contentRootPath = _env.ContentRootPath;
                string webRootPath = _env.WebRootPath;
                imagePath = "../../images/" + editOffer.ImageName;
                if (!Utility.CheckIfImageExists(_env, editOffer.ImageName))
                {
                    ModelState.AddModelError(string.Empty, "Invalid picture path");
                    return View(editOffer);
                }
                _context.OfferImages.Add(new OfferImage { Offer = offer, ImagePath = imagePath });
            }

            try
            {
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
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
            if (createOffer.ImageName == null)
            {
                _context.Offers.Add(
               new Offer
               {
                   Name = createOffer.Name,
                   Price = createOffer.Price,
                   Description = createOffer.Description

               });

            }
            else
            {
                var imagePath = "../../images/" + createOffer.ImageName;
                if (!Utility.CheckIfImageExists(_env, createOffer.ImageName))
                {
                    ModelState.AddModelError(string.Empty, "image path is incorrect");
                    return View(createOffer);
                }

                _context.OfferImages.Add(new OfferImage
                {
                    Offer = new Offer
                    {
                        Name = createOffer.Name,
                        Price = createOffer.Price,
                        Description = createOffer.Description

                    },
                    ImagePath = imagePath
                });
            }

            _context.SaveChanges();

            return View("Index");
        }
        #region review
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

            return await Task.Run<ActionResult>(() => { return RedirectToAction("Offers"); }).ConfigureAwait(false);
        }
        [Authorize(Roles = "Admin,Member")]
        #endregion

        [Authorize(Roles = "Admin,Member")]
        public IActionResult GetBookedOffers(string userId)
        {
            List<OffersViewModel> result = new List<OffersViewModel>();
            var offers = _context.Offers
                .Where(x => x.UserId == userId).ToList();
            foreach (var item in offers)
            {
                var imageSrc = _context.OfferImages.FirstOrDefault(x => x.OfferId == item.ID)?.ImagePath;

                result.Add(new OffersViewModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Price = item.Price,
                    Raiting = item.Rating,
                    IsBooked = true,
                    ImageSrc = imageSrc
                });
            }

            return View("Offers", result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private OfferViewModel GetOffer(int id)
        {
            var offer = _context.Offers.Single(x => x.ID == id);

            var imagesSrc = _context.OfferImages.Where(x => x.OfferId == id).Select(x => x.ImagePath).ToList();

            var result = new OfferViewModel
            {
                ID = offer.ID,
                Name = offer.Name,
                Description = offer.Description,
                Raiting = offer.Rating,
                Price = offer.Price,
                From = offer.AvailableFrom,
                To = offer.AvailableTo,
                ImagesSrc = imagesSrc
            };
            return result;
        }
    }
}
