using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebsite.Models
{
    public class OffersViewModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        [Range(0,double.MaxValue)]
        public double Price { get; set; }

        public double Raiting { get; set; }

        public bool IsBooked { get; set; }
    }
}
