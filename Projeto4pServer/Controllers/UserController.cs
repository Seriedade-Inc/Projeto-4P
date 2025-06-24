using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;
using System.Security.Claims;


namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            var result = await _userService.RegisterUserAsync(userDto);

            if (result == "Email já está em uso." || result == "Este nome já está em uso.")
                return Conflict(new { message = result });

            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var user = await _userService.LoginUserAsync(userDto);

            if (user == null)
                return Unauthorized("Credenciais incorretas, tente novamente.");

            // claims pro login (segurança httponly)
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in and issue cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true, // persistent cookie
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14)
                });

            // Return user info (optional)
            return Ok(new LoginResponse
    {
                UserId = user.Id, // Aqui deve ser o Guid real do usuário!
                UserName = user.UserName,
                Email = user.Email
    });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(string? username = null)
        {
            var users = await _userService.GetUsersAsync(username);

            if (users.Count == 0)
                return NotFound($"Nenhum usuário encontrado com o termo: '{username}'.");

            return Ok(users);
        }

        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            if (_userService.IsUserAuthenticated(out var userName))
            {
                return Ok(new { authenticated = true, user = userName });
            }

            return Unauthorized(new { authenticated = false });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpDelete("delete/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var result = await _userService.DeleteUserAsync(email);

            if (result == "Usuário não encontrado.")
                return NotFound(result);

            return Ok(new { message = result });
        }
    }
}