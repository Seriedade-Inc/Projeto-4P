using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;
using BCrypt.Net;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Projeto4pServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto userDto, [FromServices] EmailService emailService)
        {

            if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
                return BadRequest("Não se esqueça de inserir um email ou senha.");

            if (_context.User.Any(u => u.Email == userDto.Email))
                return BadRequest("Email já está em uso.");

            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _context.User.Add(user);
            _context.SaveChanges();

            string subject = "Bem-vindo ao nosso sistema, eu fiz isso por código!";
            string body = $"Olá {user.Email},<br><br>Seu registro foi concluído com sucesso!";
            emailService.SendEmail(user.Email, subject, body);

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
            var user = _context.User.FirstOrDefault(u => u.Email == userDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                return Unauthorized("Email ou senha incorretos.");

            return Ok(new { message = "Login bem-sucedido!" });
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteUser(int id){
            var user = _context.User.Find(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");
            
            _context.User.Remove(user);
            _context.SaveChanges();

            return Ok(new { message = $"Id de número: {id} deletado com sucesso!" });
        }
    }
}