using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;
using System.Net.Mail;
using System.Security.Cryptography;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private static Dictionary<string, string> resetCodes = new Dictionary<string, string>();
        private string Generate6CharacterCode()
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

        public UserController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto userDto, [FromServices] EmailService emailService)
        {

            if (string.IsNullOrEmpty(userDto.UserName))
                return BadRequest("Não se esqueça de inserir um nome");

            if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
                return BadRequest("Não se esqueça de inserir um email ou senha.");

            // if (_context.User.Any(u => u.Email == userDto.Email))
            //     return BadRequest("Email já está em uso.");

            if (_context.User.Any(u => u.UserName == userDto.UserName))
                return BadRequest("Este nome já está em uso.");

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            string subject = "Bem-vindo ao nosso sistema, eu fiz isso por código!";
            string body = $"Olá {user.UserName},<br><br>Seu registro foi concluído com sucesso!";
            try{
                emailService.SendEmail(user.Email, subject, body);
            }

            catch (SmtpException ex){
                return StatusCode(500, $"Erro ao enviar o e-mail: {ex.Message}");
            }

            catch{
                return BadRequest("Insira um email válido.");
            }
            
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

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto userDto)
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

            var cookieOptions = new CookieOptions { HttpOnly = true };
            if (userDto.RememberMe)
            {
                cookieOptions.Expires = DateTime.UtcNow.AddMonths(1);
            }
            Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);

            // Log the cookie setting for debugging
            Console.WriteLine($"Cookie set: UserId={user.Id}, Expires={cookieOptions.Expires}");

            return Ok(new { message = "Login bem-sucedido!" });
        }

        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            if (Request.Cookies.TryGetValue("UserId", out var userId))
            {
                var user = _context.User.Find(int.Parse(userId));
                if (user != null)
                {
                    return Ok(new { message = "Usuário autenticado", userId = user.Id });
                }
            }

            return Unauthorized("Usuário não autenticado");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Remove the authentication cookie
            if (Request.Cookies.ContainsKey("UserId"))
            {
                Response.Cookies.Delete("UserId");
            }

            return Ok(new { message = "Logout bem-sucedido!" });
        }

        [HttpPost("request-password-reset")]
        public IActionResult RequestPasswordReset([FromBody] UserEmailDto userEmailDto, [FromServices] EmailService emailService)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == userEmailDto.Email);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var resetCode = Generate6CharacterCode();
            user.ResetCode = resetCode;
            user.ResetCodeExpiration = DateTime.UtcNow.AddMinutes(15);
            _context.SaveChanges();

            string subject = "Código de redefinição de senha";
            string body = $"Olá {user.UserName},<br><br>Seu código de redefinição de senha é: {resetCode}<br><br>Seu código irá expirar em 15 minutos!";

            try
            {
                emailService.SendEmail(user.Email, subject, body);
            }
            catch (SmtpException ex)
            {
                return StatusCode(500, $"Erro ao enviar o e-mail: {ex.Message}");
            }
            catch
            {
                return BadRequest("Insira um email válido.");
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

        [HttpDelete("delete/{Id}")]
        public IActionResult DeleteUser(Guid Id){
            var user = _context.User.Find(Id);

            if (user == null)
                return NotFound("Usuário não encontrado.");
            
            _context.User.Remove(user);
            _context.SaveChanges();

            return Ok(new { message = $"Id de número: {Id} deletado com sucesso!" });
        }
    }
}