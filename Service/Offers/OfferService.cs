using Database.Data;
using Microsoft.AspNetCore.Identity;
using Models.OfferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Service.Offers
{
    public class OfferService : IOfferService
    {
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _manager;
        public OfferService(ApplicationDbContext context, UserManager<IdentityUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        public void AddOffer(CreateOfferViewModel createOffer)
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
        }

        public void BookOffer(OfferViewModel bookOffer, ClaimsPrincipal _user)
        {
            var offer = _context.Offers.Single(x => x.ID == bookOffer.ID);

            var user = _manager.GetUserAsync(_user).Result;
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            offer.User = user;
            offer.AvailableFrom = bookOffer.From;
            offer.AvailableTo = bookOffer.To;
            _context.SaveChanges();
        }

        public List<OffersViewModel> GetBooked(string userId)
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

            return result;
        }

        public EditOfferViewModel GetEditOffer(int id)
        {
            var offer = _context.Offers.Single(x => x.ID == id);
            var result = new EditOfferViewModel
            {
                ID = offer.ID,
                Name = offer.Name,
                Description = offer.Description,
                Price = offer.Price
            };
            return result;
        }

        public OfferViewModel GetOffer(int id)
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

        public IEnumerable<OffersViewModel> GetOffers()
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
            return result;
        }

        public void PostEditOffer(EditOfferViewModel editOffer)
        {
            var offer = _context.Offers.Single(x => x.ID == editOffer.ID);
            string imagePath = string.Empty;

            offer.Description = editOffer.Description;
            offer.Name = editOffer.Name;
            offer.Price = editOffer.Price;
            if (!string.IsNullOrWhiteSpace(editOffer.ImageName))
            {
                imagePath = "../../images/" + editOffer.ImageName;
                _context.OfferImages.Add(new OfferImage { Offer = offer, ImagePath = imagePath });
            }

            _context.SaveChanges();
        }
    }
}
