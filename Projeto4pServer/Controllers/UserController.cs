using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using BCrypt.Net;
using System.Linq;

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
        public IActionResult Register([FromBody] UserRegisterDto userDto)
        {
            if (_context.UserLogins.Any(u => u.Email == userDto.Email))
                return BadRequest("Email já está em uso.");

            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _context.UserLogins.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Usuário registrado com sucesso!", userId = user.Id });
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.UserLogins.ToList();
            return Ok(users);
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto userDto)
        {
            var user = _context.UserLogins.FirstOrDefault(u => u.Email == userDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                return Unauthorized("Email ou senha incorretos.");

            return Ok(new { message = "Login bem-sucedido!" });
        }
    }
}