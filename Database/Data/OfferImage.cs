using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Data
{
    public class OfferImage
    {
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
        public string ImagePath { get; set; }
    }
}
