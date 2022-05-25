using May_25_Task.Models;
using May_25_Task.ViewModels.PricingViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May_25_Task.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Icon> Icons { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<PricingPlan> PricingPlans { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
