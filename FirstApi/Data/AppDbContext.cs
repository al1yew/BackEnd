using FirstApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Data
{
    public class AppDbContext : DbContext
    {
        //Add-Migration --name of migration-- -OutputDir Data/Migrations - first initial catalog must be written as shown
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
    }
}
