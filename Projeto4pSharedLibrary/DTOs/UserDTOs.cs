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
        public bool RememberMe { get; set; }
    }
    public class UserEmailDto
    {
        public string Email { get; set; } = string.Empty;
    }

    public class UserResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;
        
        public string ResetCode { get; set; } = string.Empty;
        
        public string NewPassword { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        // Add other fields if needed (e.g., Token, UserId, etc.)
    }

}
