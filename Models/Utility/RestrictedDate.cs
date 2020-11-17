using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Utility
{
    public class RestrictedDate : ValidationAttribute
    {
        public override bool IsValid(object date)
        {
            DateTime d = Convert.ToDateTime(date);
            return d >= DateTime.Now;
        }
    }
}
