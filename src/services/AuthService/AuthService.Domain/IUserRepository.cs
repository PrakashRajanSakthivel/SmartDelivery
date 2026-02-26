namespace AuthService.Domain
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);
    }
}
