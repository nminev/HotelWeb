using HotelWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebsite.Models
{
    public class CreateReviewViewModel
    {
        public string OfferName { get; set; }

        public int OfferId { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public int Score { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
