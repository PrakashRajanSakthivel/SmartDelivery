using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurentService.Domain.Entites;


namespace RestaurentService.Infra.Configuration
{
    public class RestaurentConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurent");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).IsRequired().HasMaxLength(100);
        }
    }
}
