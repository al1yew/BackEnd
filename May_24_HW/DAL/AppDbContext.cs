﻿using Microsoft.EntityFrameworkCore;
using May_24_HW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_24_HW.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PricingPlan> PricingPlans { get; set; }
        
    }
}
