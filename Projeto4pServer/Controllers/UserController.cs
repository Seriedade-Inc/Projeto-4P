using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;


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

            return Ok(new { message = "Login bem-sucedido", userId = user.Id, userName = user.UserName });
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
            await _userService.LogoutAsync();
            return Ok(new { message = "Logged out" });
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