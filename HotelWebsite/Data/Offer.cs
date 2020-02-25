using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HotelWebsite.Data
{
    public class Offer
    {
        public int ID { get; set; }
        public int Price { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Review { get; set; }

        public double Rating { get; set; }

    }
}
