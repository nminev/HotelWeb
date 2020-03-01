using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebsite.Models
{
    public class OfferViewModel
    {
        private double price;

        public int ID { get; set; }
        [Range(0, int.MaxValue)]
        public double Price { get{ return Math.Round(price, 2); } set => price = value; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }

        public double Raiting { get; set; }

        public int TotalReviews { get; set; }

        public DateTime AvailableFrom { get; set; }

        public DateTime AvailableTo { get; set; }

        public bool IsBooked { get; set; }
    }
}
