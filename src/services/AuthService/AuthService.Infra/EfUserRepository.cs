using AuthService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infra
{
    public class EfUserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public EfUserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            await _context.Users.AddAsync(user);
        }
    }
}
