using AuthService.Domain;
using Shared.Data.Repositories;

namespace AuthService.Infra
{
    public class AuthUnitOfWork : UnitOfWork<AuthDbContext>, IAuthUnitOfWork
    {
        public IUserRepository Users { get; }

        public AuthUnitOfWork(AuthDbContext context, IUserRepository users) : base(context)
        {
            Users = users;
        }
    }
}
