using AuthService.Domain;
using BCrypt.Net;

namespace AuthService.Infra
{
    public class AuthService : IAuthService
    {
        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        // For now, just return a dummy token
        public string GenerateJwtToken(User user)
        {
            return "dummy-jwt-token";
        }
    }

    public interface IAuthService
    {
        bool VerifyPassword(string password, string hash);
        string GenerateJwtToken(User user);
    }
}
