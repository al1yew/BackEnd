using FirstApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(255);
            builder.Property(x => x.SurName).HasMaxLength(255);
            builder.Property(x => x.UserName).HasMaxLength(255);
            builder.Property(x => x.Email).HasMaxLength(255);
        }
    }
}
