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
            var user = _manager.GetUserAsync(_user).Result;
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            var booking = new Bookings
            {
                From = bookOffer.From,
                To = bookOffer.To,
                User = user,
                OfferId = bookOffer.OfferID
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public List<OffersViewModel> GetBooked(string userId)
        {
            List<OffersViewModel> result = new List<OffersViewModel>();
            var bookings = _context.Bookings
                .Where(x => x.UserId == userId).ToList();

            var offers = (bookings != null || bookings.Any()) ? _context.Offers.Where(x => bookings.Any(y => y.OfferId == x.ID)) : null;
            foreach (var item in offers)
            {
                var imageSrc = _context.OfferImages.FirstOrDefault(x => x.OfferId == item.ID)?.ImagePath;

                result.Add(new OffersViewModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Price = item.Price,
                    Raiting = item.Rating,
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
            var offer = _context.Offers.SingleOrDefault(x => x.ID == id);
            var booking = _context.Bookings.SingleOrDefault(x => x.ID == id);

            var imagesSrc = _context.OfferImages.Where(x => x.OfferId == id).Select(x => x.ImagePath).ToList();

            var result = new OfferViewModel
            {
                OfferID = offer.ID,
                Name = offer.Name,
                Description = offer.Description,
                Raiting = offer.Rating,
                Price = offer.Price,
                ImagesSrc = imagesSrc
            };

            return result;
        }

        public IEnumerable<OffersViewModel> GetOffers()
        {
            var result = new List<OffersViewModel>();
            foreach (var item in _context.Offers)
            {
                var imageSrc = _context.OfferImages.FirstOrDefault(x => x.OfferId == item.ID)?.ImagePath;

                result.Add(new OffersViewModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Price = item.Price,
                    Raiting = item.Rating,
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
