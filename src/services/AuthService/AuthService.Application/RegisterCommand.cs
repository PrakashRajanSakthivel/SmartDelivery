using MediatR;
using AuthService.Application;

namespace AuthService.Application
{
    public record RegisterCommand(string Username, string Password) : IRequest<AuthResponse>;
}