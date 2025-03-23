namespace Projeto4pServer.Data
{
    public class User
    {
        public string UserName { get; set;} = string.Empty;
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}