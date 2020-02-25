using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HotelWebsite.Models;
using HotelWebsite.Data;

namespace HotelWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<Offer> InMemoryStore;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            InMemoryStore = new List<Offer>()
            {
                new Offer {
                    ID=0,
                    Name = "Offer One",
                    Description = "Fist offer to see how it will look",
                    Price=123
                },
                new Offer{
                    ID=1,
                    Name = "This is Two",
                    Description = "This is the second offer, purely for testing purpouses",
                    Price = 332
                }
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Offers()
        {
            var result = new List<OffersViewModel>();
            foreach (var item in InMemoryStore)
            {
                result.Add(new OffersViewModel { ID = item.ID, Name = item.Name });
            }
            return View(result);
        }

        public IActionResult Offer(int id)
        {
            var offer = InMemoryStore.Single(x => x.ID == id);
            var result = new OfferViewModel
            {
                ID = offer.ID,
                Name = offer.Name,
                Description = offer.Description,
                Rating = offer.Rating,
                Review = offer.Review,
                Price = offer.Price
            };
            return View(result);
        }

        public IActionResult EditOffer(int id)
        {
            var offer = InMemoryStore.Single(x => x.ID == id);
            var result = new EditOfferViewModel
            {
                ID = offer.ID,
                Name = offer.Name,
                Description = offer.Description,
                Price = offer.Price
            };
            return View(result);
        }

        [HttpPut]
        public IActionResult PutOffer(EditOfferViewModel editOffer)
        {
            var offer = InMemoryStore.Single(x => x.ID == editOffer.ID);

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
