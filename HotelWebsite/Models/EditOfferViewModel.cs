﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebsite.Models
{
    public class EditOfferViewModel
    {
        public int ID { get; set; }
        public int Price { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
