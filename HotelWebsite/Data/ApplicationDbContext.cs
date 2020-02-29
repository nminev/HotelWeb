using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelWebsite.Models;

namespace HotelWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Offer> Offers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            if (Database.EnsureCreated())
            {
                SeedData();
            }
        }

        private void SeedData()
        {
            this.Offers.Add(
                new Offer
                {
                    Name = "Offer One",
                    Description = "Fist offer to see how it will look",
                    Price = 123
                });

            this.Offers.Add(
                new Offer
                {
                    Name = "This is Two",
                    Description = "This is the second offer, purely for testing purpouses",
                    Price = 332
                });

            this.SaveChanges();
        }

        public DbSet<HotelWebsite.Models.EditOfferViewModel> EditOfferViewModel { get; set; }
    }
}
