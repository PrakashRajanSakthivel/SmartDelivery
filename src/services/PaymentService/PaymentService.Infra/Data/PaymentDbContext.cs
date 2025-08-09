using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entites;

namespace PaymentService.Infra.Data
{
    public class PaymentDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);
                
                // Configure properties
                entity.Property(p => p.PaymentIntentId)
                      .IsRequired()
                      .HasMaxLength(100);
                      
                entity.Property(p => p.Currency)
                      .IsRequired()
                      .HasMaxLength(3)
                      .HasDefaultValue("USD");
                      
                entity.Property(p => p.Amount)
                      .HasColumnType("decimal(18,2)");
                      
                entity.Property(p => p.Status)
                      .HasConversion<string>()
                      .IsRequired();
                      
                // Configure indexes
                entity.HasIndex(p => p.PaymentIntentId)
                      .IsUnique();
                      
                entity.HasIndex(p => p.OrderId);
                entity.HasIndex(p => p.UserId);
                entity.HasIndex(p => p.Status);
                entity.HasIndex(p => p.CreatedAt);

            });
        }
    }
}