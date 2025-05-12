using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurentService.Domain.Entites;

namespace RestaurentService.Infra.Data
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Restaurant configuration
            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasKey(r => r.Id);

                // Basic properties
                entity.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(r => r.Description)
                    .HasMaxLength(500);

                entity.Property(r => r.CoverImageUrl)
                    .HasMaxLength(255);

                entity.Property(r => r.LogoUrl)
                    .HasMaxLength(255);

                entity.Property(r => r.Address)
                    .HasMaxLength(200);

                entity.Property(r => r.DeliveryFee)
                    .HasColumnType("decimal(18,2)");

                entity.Property(r => r.MinOrderAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(r => r.AverageRating)
                    .HasPrecision(3, 2);

                entity.Property(r => r.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                // Indexes for performance
                entity.HasIndex(r => r.Name);
                entity.HasIndex(r => r.AverageRating);
                entity.HasIndex(r => r.EstimatedDeliveryTime);
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.DisplayOrder)
                    .HasDefaultValue(0);

                // Relationships
                entity.HasOne(c => c.Restaurant)
                    .WithMany(r => r.Categories)
                    .HasForeignKey(c => c.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(c => new { c.RestaurantId, c.DisplayOrder });
            });

            // MenuItem configuration
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(m => m.Id);

                // Basic properties
                entity.Property(m => m.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(m => m.Description)
                    .HasMaxLength(500);

                entity.Property(m => m.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(m => m.ImageUrl)
                    .HasMaxLength(255);

                entity.Property(m => m.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                // Relationships
                entity.HasOne(m => m.Restaurant)
                    .WithMany(r => r.MenuItems)
                    .HasForeignKey(m => m.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Category)
                    .WithMany(c => c.MenuItems)
                    .HasForeignKey(m => m.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Indexes for filtering
                entity.HasIndex(m => m.Name);
                entity.HasIndex(m => new { m.RestaurantId, m.CategoryId });
                entity.HasIndex(m => m.IsVegetarian);
                entity.HasIndex(m => m.IsVegan);
                entity.HasIndex(m => m.IsAvailable);
            });

            // Seed initial data if needed
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                modelBuilder.Entity<Restaurant>().HasData(
                    new Restaurant
                    {
                        Id = Guid.NewGuid(),
                        Name = "Burger Palace",
                        Description = "Home of the best burgers in town",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        AverageRating = 4.5,
                        DeliveryFee = 2.99m,
                        MinOrderAmount = 10.00m,
                        EstimatedDeliveryTime = 30
                    }
                );
            }
        }
    }
}
