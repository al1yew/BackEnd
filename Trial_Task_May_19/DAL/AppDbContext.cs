using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trial_Task_May_19.Models;

namespace Trial_Task_May_19.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Brand> Brands { get; set; }

        public DbSet<Car> Cars { get; set; }
    }
}
