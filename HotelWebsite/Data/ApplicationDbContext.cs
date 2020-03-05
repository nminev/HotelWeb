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

        public DbSet<Review> Reviews { get; set; }

        public DbSet<OfferImage> OfferImages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            if (Database.EnsureCreated())
            {
                SeedData();
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<OfferImage>()
                       .HasKey(x => new { x.OfferId, x.ImagePath });

            base.OnModelCreating(builder);
        }

        private void SeedData()
        {
            var offerOne = new Offer
            {
                Name = "Costa Rica",
                Description = "Pretty Hotel in Costa Rica come and see it ",
                Price = 123
            };

            var offerTwo = new Offer
            {
                Name = "This is Two",
                Description = "This is purely for testing purpouses",
                Price = 332
            };

            var offerThree = new Offer
            {
                Name = "This is Two",
                Description = "This is purely for testing purpouses",
                Price = 332
            };



            var offerFour = new Offer
            {
                Name = "This is Three",
                Description = "This is purely for testing purpouses",
                Price = 332
            };

            this.OfferImages.Add(new OfferImage
            {
                ImagePath = "../../images/Hotel-placeholder-1.png",
                Offer= offerOne
            });
            this.OfferImages.Add(new OfferImage
            {
                ImagePath = "../../images/hotel-placeholder-2.png",
                Offer = offerOne
            });
            this.OfferImages.Add(new OfferImage
            {
                ImagePath = "../../images/Hotel-placeholder-1.png",
                Offer = offerTwo
            });
            this.OfferImages.Add(new OfferImage
            {
                ImagePath = "../../images/Hotel-placeholder-1.png",
                Offer = offerThree
            });
            this.OfferImages.Add(new OfferImage
            {
                ImagePath = "../../images/Hotel-placeholder-1.png",
                Offer = offerFour
            });
            this.SaveChanges();
        }
    }
}
