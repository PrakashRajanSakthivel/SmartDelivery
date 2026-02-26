namespace AuthService.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public AuthProvider Provider { get; set; } = AuthProvider.EmailPassword;
    }

    public enum AuthProvider
    {
        EmailPassword = 0,
        Google = 1
    }
}
