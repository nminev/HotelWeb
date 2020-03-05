using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebsite.Models
{
    public class EditOfferViewModel:CreateOfferViewModel
    {
        [Required]
        public int ID { get; set; }
    }
}
