using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebsite.Models
{
    public class RestrictedDate : ValidationAttribute
    {
        public override bool IsValid(object date)
        {
            if( date != null)
            {
                return false;
            }
            DateTime d = Convert.ToDateTime(date);
            return d >= DateTime.Now;
        }
    }
}
