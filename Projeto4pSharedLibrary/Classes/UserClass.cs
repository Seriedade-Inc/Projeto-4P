namespace Projeto4pSharedLibrary.Classes
{
    public class User
    {
        public string UserName { get; set;} = string.Empty;
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? ResetCode { get; set; } = string.Empty;
        public DateTime? ResetCodeExpiration { get; set; }
    }
}