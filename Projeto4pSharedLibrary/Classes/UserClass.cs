namespace Projeto4pSharedLibrary.Classes
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<Character>? Characters { get; set; } = new List<Character>();
        public string? ResetCode { get; set; } = string.Empty; // * Pensar em fazer no front, definir ainda
        public DateTime? ResetCodeExpiration { get; set; } // * Pensar em fazer no front, definir ainda
    }
}