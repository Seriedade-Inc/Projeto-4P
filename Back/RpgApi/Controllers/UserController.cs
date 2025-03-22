using Microsoft.AspNetCore.Mvc;
using RpgApi.Models;
using RpgApi.DTOs;
using BCrypt.Net;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        
        private static List<User> Users = new List<User>();


        [HttpPost("register")]
        public IActionResult Register(UserRegisterDto userDto)
        {
            
            if (Users.Any(u => u.Email == userDto.Email))
                return BadRequest("Email já está em uso.");

            
            var user = new User
            {
                Id = Users.Count + 1, 
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            
            Users.Add(user);

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(Users);
        }

        
        [HttpPost("login")]
        public IActionResult Login(UserLoginDto userDto)
        {
            
            var user = Users.FirstOrDefault(u => u.Email == userDto.Email);

            
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                return Unauthorized("Email ou senha incorretos.");

            
            return Ok(new { message = "Login bem-sucedido!" });
        }
    }
}