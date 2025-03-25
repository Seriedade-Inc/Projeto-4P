namespace Projeto4pSharedLibrary.Classes
{
    public class User
    {
        
        public Ulid Id { get; set; } = Ulid.NewUlid();
        public string UserName { get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? ResetCode { get; set; } = string.Empty;
        public DateTime? ResetCodeExpiration { get; set; }
    }
}