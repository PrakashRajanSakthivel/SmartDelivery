using OrderService.Domain.Entites;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace OrderService.Infra.Data
{
    using Microsoft.EntityFrameworkCore;

    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatusHistory> StatusHistories { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.Status).HasConversion<string>();

                entity.HasMany(o => o.OrderItems)
                      .WithOne(i => i.Order)
                      .HasForeignKey(i => i.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.StatusHistories)
                      .WithOne(s => s.Order)
                      .HasForeignKey(s => s.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(i => i.OrderItemId);
            });

            modelBuilder.Entity<OrderStatusHistory>(entity =>
            {
                entity.HasKey(s => s.StatusId);
                entity.Property(s => s.Status).HasConversion<string>();
            });
        }
    }
}