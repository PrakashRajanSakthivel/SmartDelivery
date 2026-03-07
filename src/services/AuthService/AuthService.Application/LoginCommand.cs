using MediatR;
using AuthService.Application;

namespace AuthService.Application
{
    public record LoginCommand(string Username, string Password) : IRequest<AuthResponse>;
}
