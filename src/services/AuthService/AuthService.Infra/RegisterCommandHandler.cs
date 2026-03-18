using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application;
using AuthService.Domain;

namespace AuthService.Infra
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IAuthUnitOfWork _uow;
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthUnitOfWork uow, IAuthService authService)
        {
            _uow = uow;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _uow.Users.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                return new AuthResponse { Success = false, Message = "Username already exists" };
            }

            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsActive = true,
                Provider = AuthProvider.EmailPassword
            };

            await _uow.Users.AddUserAsync(newUser);
            await _uow.CommitAsync(cancellationToken);

            var token = _authService.GenerateJwtToken(newUser);
            var userDto = new UserDto { UserId = newUser.Id, Username = newUser.Username, IsActive = newUser.IsActive };
            return new AuthResponse { Success = true, Token = token, User = userDto, Message = "Registration successful" };
        }
    }
}