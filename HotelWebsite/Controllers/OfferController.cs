using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Database.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Hosting;
using Models.OfferModels;
using Models.Utility;
using Microsoft.AspNetCore.Http;
using Service.Offers;

namespace HotelWebsite.Controllers
{
    public class OfferController : Controller
    {
        private readonly ILogger<OfferController> _logger;
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _manager;
        private readonly IWebHostEnvironment _env;
        private readonly IOfferService _offerService;

        public OfferController(ILogger<OfferController> logger, ApplicationDbContext context, UserManager<IdentityUser> manager, IWebHostEnvironment env, IOfferService offerService)
        {
            _logger = logger;
            _context = context;
            _manager = manager;
            _env = env;
            _offerService = offerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Member")]
        public IActionResult Offers()
        {
            return View(GetOffers());
        }

        [Authorize(Roles = "Admin,Member")]
        [HttpGet]
        public IActionResult Offer(int id)
        {
            return View(_offerService.GetOffer(id));
        }

        [HttpPost]
        public async Task<IActionResult> Offer(OfferViewModel bookOffer)
        {
            if (!this.ModelState.IsValid)
            {
                if (bookOffer.To < bookOffer.From)
                {
                    ModelState.AddModelError(string.Empty, "The \"From\" date cannot be smaller then the \"To\" date");
                }
                bookOffer = _offerService.GetOffer(bookOffer.ID);
                return View(bookOffer);
            }

            try
            {
                _offerService.BookOffer(bookOffer, HttpContext.User);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return await Task.Run<ActionResult>(() => { return RedirectToAction("Offer", "Offer", new { id = bookOffer.ID }); }).ConfigureAwait(false);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditOffer(int id)
        {
            return View(_offerService.GetEditOffer(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditOffer(EditOfferViewModel editOffer)
        {
            if (!Utility.CheckIfImageExists(_env, editOffer.ImageName))
            {
                ModelState.AddModelError(string.Empty, "Invalid picture path");
                return View(editOffer);
            }

            _offerService.PostEditOffer(editOffer);

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
            if (!Utility.CheckIfImageExists(_env, createOffer.ImageName))
            {
                ModelState.AddModelError(string.Empty, "image path is incorrect");
                return View(createOffer);
            }
            _offerService.AddOffer(createOffer);

            return View("Index");
        }

        [Authorize(Roles = "Admin,Member")]
        public IActionResult GetBookedOffers(string userId)
        {
            List<OffersViewModel> result = _offerService.GetBooked(userId);

            return View("Offers", result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
