using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Projeto4pServer.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> RegisterUserAsync(UserRegisterDto userDto)
        {
            if (_context.User.Any(u => u.Email == userDto.Email))
                return "Email já está em uso.";

            if (_context.User.Any(u => u.UserName.ToLower() == userDto.UserName.ToLower()))
                return "Este nome já está em uso.";

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return "Usuário registrado com sucesso!";
        }

        public async Task<User?> LoginUserAsync(UserLoginDto userDto)
        {
            User? user = null;

            if (!string.IsNullOrEmpty(userDto.Email))
            {
                user = await _context.User.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            }
            else if (!string.IsNullOrEmpty(userDto.UserName))
            {
                user = await _context.User.FirstOrDefaultAsync(u => u.UserName == userDto.UserName);
            }

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public async Task<List<User>> GetUsersAsync(string? username = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                return await _context.User.ToListAsync();
            }

            return await _context.User
                .Where(u => u.UserName.Contains(username, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }
        
        public bool IsUserAuthenticated(out string? userName)
        {
          var httpContext = _httpContextAccessor.HttpContext;

          if (httpContext?.User.Identity?.IsAuthenticated == true)
          {
              userName = httpContext.User.Identity.Name;
              return true;
          }

          userName = null;
          return false;
        }

         public async Task LogoutAsync()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        public async Task<string> DeleteUserAsync(string email)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return "Usuário não encontrado.";

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return $"O email: {email} foi deletado com sucesso!";
        }
    }
}