using Shared.Data.Interfaces;

namespace AuthService.Domain
{
    public interface IAuthUnitOfWork : IUnitOfWork
    {
        IUserRepository Users { get; }
    }
}
