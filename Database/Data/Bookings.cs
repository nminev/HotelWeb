using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Data
{
    public class Bookings
    {
        [Key]
        public int ID { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public virtual Offer Offer { get; set; }

        [ForeignKey("Offer")]
        public virtual int OfferId { get; set; }

        public virtual IdentityUser User { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
