using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;
using Projeto4pSharedLibrary.Classes;

    
namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(AppDbContext context, EmailService emailService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly EmailService _emailService = emailService;
        private static string Generate6CharacterCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            byte[] data = new byte[6];
            RandomNumberGenerator.Fill(data);
            char[] result = new char[6];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[data[i] % chars.Length];
            }
            return new string(result);
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto userDto, [FromServices] EmailService emailService)
        {

            if (string.IsNullOrEmpty(userDto.UserName))
                return BadRequest("Não se esqueça de inserir um nome");

            if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
                return BadRequest("Não se esqueça de inserir um email ou senha.");

            if (_context.User.Any(u => u.Email == userDto.Email))
                return BadRequest("Email já está em uso.");

            if (_context.User.Any(u => u.UserName.ToLower() == userDto.UserName.ToLower()))
                return BadRequest("Este nome já está em uso.");

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            // ! Esperar o aws ser configurado

            // string subject = "Bem-vindo ao nosso sistema, eu fiz isso por código!";
            // string body = $"Olá {user.UserName},<br><br>Seu registro foi concluído com sucesso!";
            // try{
            //     _ = _emailService.SendEmailAsync(user.Email, subject, body);
            // }

            // catch (Exception ex){
            //     return StatusCode(500, $"Erro ao registrar o e-mail: {ex.Message}");
            // }
  
            _context.User.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Usuário registrado com sucesso!", userId = user.Id });
        }

        [HttpGet]
        public IActionResult GetUsers()
        {   
            var users = _context.User.ToList();
            return Ok(users);                       
        }

        [HttpGet("{username}")]
        public IActionResult GetByUserName(string? username = null)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest("Username cannot be null or empty.");

            var users = _context.User
                .AsEnumerable() // Forces client-side evaluation
                .Where(u => u.UserName.Contains(username, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(u => u.UserName)
                .ToList();

            if (users.Count == 0)
                return NotFound($"Nenhum usuário encontrado com o termo: '{username}'.");

            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginDto userDto)
        {
            User? user = null;

            if (!string.IsNullOrEmpty(userDto.Email))
            {
                user = _context.User.FirstOrDefault(u => u.Email == userDto.Email);
            }
            else if (!string.IsNullOrEmpty(userDto.UserName))
            {
                user = _context.User.FirstOrDefault(u => u.UserName == userDto.UserName);
            }

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return Unauthorized("Credenciais incorretas, tente novamente.");
            }

            var sessionId = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new("SessionId", sessionId),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email)
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
             );

            return Ok(new { message = "Login successful" });
        }

        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            if ((User.Identity?.IsAuthenticated) != true)
            {
                return Unauthorized(new { authenticated = false });
            }
            return Ok(new { authenticated = true, user = User.Identity.Name});
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out" });
        } 

        [HttpPost("request-password-reset")]
        public IActionResult RequestPasswordReset([FromBody] UserEmailDto userEmailDto, [FromServices] EmailService emailService)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == userEmailDto.Email);

            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }
            var resetCode = Generate6CharacterCode();
            user.ResetCode = resetCode;
            user.ResetCodeExpiration = DateTime.UtcNow.AddMinutes(15);
            _context.SaveChanges();

            string subject = "Código de redefinição de senha";
            string body = $"Olá {user.UserName},<br><br>Seu código de redefinição de senha é: {resetCode}<br><br>Seu código irá expirar em 15 minutos!";

            try
            {
                _ = _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar o e-mail: {ex.Message}");
            }

            return Ok(new { message = "Código de redefinição de senha enviado com sucesso!" });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] UserResetPasswordDto resetPasswordDto)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == resetPasswordDto.Email);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            if (user.ResetCode != resetPasswordDto.ResetCode)
                return Unauthorized("Código de redefinição inválido.");

            if (user.ResetCodeExpiration == null || user.ResetCodeExpiration < DateTime.UtcNow)
                return Unauthorized("Código de redefinição expirado.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
            user.ResetCode = null;
            user.ResetCodeExpiration = null;
            _context.SaveChanges();

            return Ok(new { message = "Senha alterada com sucesso!" });
        }

        [HttpDelete("delete/{email}")]
        public IActionResult DeleteUser(string email){
            var user = _context.User.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return NotFound("Usuário não encontrado.");
            
            _context.User.Remove(user);
            _context.SaveChanges();

            return Ok(new { message = $"O email: {email} foi deletado com sucesso!" });
        }
    }
}