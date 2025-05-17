using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entites;

namespace OrderService.Infra.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(r => r.OrderId);
            builder.Property(r => r.OrderId).IsRequired().HasMaxLength(100);
        }
    }
}
