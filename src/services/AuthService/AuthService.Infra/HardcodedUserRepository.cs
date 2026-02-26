using AuthService.Domain;

namespace AuthService.Infra
{
    public class HardcodedUserRepository : IUserRepository
    {
        private static readonly User _user = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            IsActive = true,
            Provider = AuthProvider.EmailPassword
        };

        public User? GetByUsername(string username)
        {
            return username == _user.Username ? _user : null;
        }
    }
}
