using AuthService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infra
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.Provider).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();

                // Seed users: admin/password123, testuser/password
                entity.HasData(
                    new User
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Username = "admin",
                        PasswordHash = "$2a$11$5SjYWPfuXb.Qw8dcyAR76.wb07YwR1C/S7iviSaiZnb/S12k89O4e",
                        IsActive = true,
                        Provider = AuthProvider.EmailPassword
                    },
                    new User
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Username = "testuser",
                        PasswordHash = "$2a$11$NiG6LhVwoTWjhXGoK5Aap.QCQlhmgEEyBdGLMFRlG2xHPFe.gwcfa",
                        IsActive = true,
                        Provider = AuthProvider.EmailPassword
                    }
                );
            });
        }
    }
}
