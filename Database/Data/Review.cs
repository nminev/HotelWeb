﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Data
{
    public class Review
    {
        [Key]
        public int ID { get; set; }

        public int Score { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public IdentityUser Reviewer { get; set; }

        public Offer Offer { get; set; }
    }
}
