using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application;
using AuthService.Domain;

namespace AuthService.Infra
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IUserRepository userRepo, IAuthService authService)
        {
            _userRepo = userRepo;
            _authService = authService;
        }

        public Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if user already exists
            var existingUser = _userRepo.GetByUsername(request.Username);
            if (existingUser != null)
            {
                return Task.FromResult(new AuthResponse { Success = false, Message = "Username already exists" });
            }

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create new user
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = passwordHash,
                IsActive = true,
                Provider = AuthProvider.EmailPassword
            };

            _userRepo.AddUser(newUser);

            // Generate token for immediate login
            var token = _authService.GenerateJwtToken(newUser);
            var userDto = new UserDto { UserId = newUser.Id, Username = newUser.Username, IsActive = newUser.IsActive };
            return Task.FromResult(new AuthResponse { Success = true, Token = token, User = userDto, Message = "Registration successful" });
        }
    }
}