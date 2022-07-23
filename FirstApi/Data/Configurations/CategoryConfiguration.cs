using FirstApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(255).HasColumnType("nvarchar");
            builder.Property(x => x.Image).HasMaxLength(500).HasColumnType("nvarchar");

            //builder.HasOne(x => x.Parent).WithMany(x => x.Children);
            //migration ozu relationu set edir. Id kimi
        }
    }
}
