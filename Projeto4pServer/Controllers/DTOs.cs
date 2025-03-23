namespace Projeto4pServer.DTOs

{
    public class UserRegisterDto
    {
        public string UserName { get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;           
        public string Password { get; set; } = string.Empty;
    }

    public class UserLoginDto
    {   
        public string UserName { get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
