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

        public DbSet <Review> Reviews { get; set; }
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
                    Name = "Costa Rica",
                    Description = "Pretty Hotel in Costa Rica come and see it ",
                    Price = 123,
                    AvailableFrom = DateTime.Today.AddDays(-1),
                    AvailableTo = DateTime.Today.AddDays(1)
                });

            this.Offers.Add(
                new Offer
                {
                    Name = "This is Two",
                    Description = "This is purely for testing purpouses",
                    Price = 332,
                    AvailableFrom = DateTime.Today,
                    AvailableTo = DateTime.Today.AddDays(32)
                });


            this.Offers.Add(
                new Offer
                {
                    Name = "This is Two",
                    Description = "This is purely for testing purpouses",
                    Price = 332,
                    AvailableFrom = DateTime.Today.AddDays(-2),
                    AvailableTo = DateTime.Today.AddDays(1)
                });


            this.Offers.Add(
                new Offer
                {
                    Name = "This is Three",
                    Description = "This is purely for testing purpouses",
                    Price = 332,
                    AvailableFrom = DateTime.Today.AddDays(-12),
                    AvailableTo = DateTime.Today.AddDays(111)
                });


            this.Offers.Add(
                new Offer
                {
                    Name = "This is Four",
                    Description = "This is purely for testing purpouses",
                    Price = 332,
                    AvailableFrom = DateTime.Today,
                    AvailableTo = DateTime.Today.AddDays(33)
                });


            this.Offers.Add(
                new Offer
                {
                    Name = "This is Five",
                    Description = "This is purely for testing purpouses",
                    Price = 332,
                    AvailableFrom = DateTime.Today,
                    AvailableTo = DateTime.Today.AddDays(7)
                });


            this.Offers.Add(
                new Offer
                {
                    Name = "This is Six",
                    Description = "This is purely for testing purpouses",
                    Price = 332,
                    AvailableFrom = DateTime.Today,
                    AvailableTo = DateTime.Today.AddDays(16)
                });


            this.Offers.Add(
                new Offer
                {
                    Name = "This is Seven",
                    Description = "This is purely for testing purpouses",
                    Price = 332,
                    AvailableFrom = DateTime.Today,
                    AvailableTo = DateTime.Today.AddDays(69)
                });
            this.SaveChanges();
        }
    }
}
