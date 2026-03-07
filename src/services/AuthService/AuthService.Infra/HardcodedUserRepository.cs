using AuthService.Domain;

namespace AuthService.Infra
{
    public class HardcodedUserRepository : IUserRepository
    {
        private static readonly List<User> _users =
        [
            new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                IsActive = true,
                Provider = AuthProvider.EmailPassword
            },
            new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Username = "testuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                IsActive = true,
                Provider = AuthProvider.EmailPassword
            }
        ];

        public User? GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
        }
    }
}
