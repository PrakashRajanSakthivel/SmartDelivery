using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application;
using AuthService.Domain;

namespace AuthService.Infra
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthService _authService;

        public LoginCommandHandler(IUserRepository userRepo, IAuthService authService)
        {
            _userRepo = userRepo;
            _authService = authService;
        }

        public Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepo.GetByUsername(request.Username);
            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Task.FromResult(new AuthResponse { Success = false, Message = "Invalid credentials" });
            }

            var token = _authService.GenerateJwtToken(user);
            var userDto = new UserDto { Username = user.Username, IsActive = user.IsActive };
            return Task.FromResult(new AuthResponse { Success = true, Token = token, User = userDto, Message = "Login successful" });
        }
    }
}
