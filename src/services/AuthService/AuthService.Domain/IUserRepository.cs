namespace AuthService.Domain
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);
        void AddUser(User user);
    }
}
