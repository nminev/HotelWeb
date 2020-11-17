using Models.OfferModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Service.Offers
{
    public interface IOfferService
    {
        OfferViewModel GetOffer(int id);
        IEnumerable<OffersViewModel> GetOffers();
        void BookOffer(OfferViewModel bookOffer, ClaimsPrincipal user);
        EditOfferViewModel GetEditOffer(int id);
        void AddOffer(CreateOfferViewModel createOffer);
        List<OffersViewModel> GetBooked(string userId);
        void PostEditOffer(EditOfferViewModel editOffer);
    }
}
