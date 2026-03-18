using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application;
using AuthService.Domain;

namespace AuthService.Infra
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IAuthUnitOfWork _uow;
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthUnitOfWork uow, IAuthService authService)
        {
            _uow = uow;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByUsernameAsync(request.Username);
            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return new AuthResponse { Success = false, Message = "Invalid credentials" };
            }

            var token = _authService.GenerateJwtToken(user);
            var userDto = new UserDto { UserId = user.Id, Username = user.Username, IsActive = user.IsActive };
            return new AuthResponse { Success = true, Token = token, User = userDto, Message = "Login successful" };
        }
    }
}
