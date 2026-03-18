using Microsoft.EntityFrameworkCore;
using PaymentService.Domain;

namespace PaymentService.Infra
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

        public DbSet<PaymentIntentRecord> PaymentIntents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentIntentRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ClientSecret).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}
