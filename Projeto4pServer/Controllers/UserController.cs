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
             try
            {
                var result = await _userService.RegisterUserAsync(userDto);
                if (result == "Email já está em uso." || result == "Este nome já está em uso.")
                    return Conflict(new { message = result });
                if (result != "Usuário registrado com sucesso!")
                    return StatusCode(500, new { message = result });
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor.", detail = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            try
            {
                var user = await _userService.LoginUserAsync(userDto);
                if (user == null)
                    return Unauthorized("Credenciais incorretas, tente novamente.");
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Email, user.Email)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14) });
                return Ok(new LoginResponse
<<<<<<< HEAD
                    {
                UserId = user.Id, // Aqui deve ser o Guid real do usuário!
                UserName = user.UserName,
                Email = user.Email
                                 });
=======
                {
                            UserId = user.Id, // Aqui deve ser o Guid real do usuário!
                            UserName = user.UserName,
                            Email = user.Email
                });
>>>>>>> 8a797beee3a27579ea5ae3c76ba31990284090c8
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor.", detail = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(string? username = null)
        {
            try
            {
                var users = await _userService.GetUsersAsync(username);
                if (users.Count == 0)
                    return NotFound($"Nenhum usuário encontrado com o termo: '{username}'");
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor.", detail = ex.Message });
            }
        }

        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            try
            {
                if (_userService.IsUserAuthenticated(out var userName))
                    return Ok(new { authenticated = true, user = userName });
                return Unauthorized(new { authenticated = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor.", detail = ex.Message });
            }
        }

        [HttpDelete("delete/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(email);
                if (result == "Usuário não encontrado.")
                    return NotFound(result);
                if (!result.Contains("deletado com sucesso"))
                    return StatusCode(500, new { message = result });
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor.", detail = ex.Message });
            }
        }
    }
}