using Projeto4pServer.DTOs;
using Projeto4pServer.Repository;
using Projeto4pSharedLibrary.Classes;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Projeto4pServer.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> RegisterUserAsync(UserRegisterDto userDto)
        {
            if (await _userRepository.EmailExistsAsync(userDto.Email))
                return "Email já está em uso.";

            if (await _userRepository.UsernameExistsAsync(userDto.UserName))
                return "Este nome já está em uso.";

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            await _userRepository.CreateAsync(user);
            return "Usuário registrado com sucesso!";
        }

        public async Task<User?> LoginUserAsync(UserLoginDto userDto)
        {
            User? user = null;
            if (!string.IsNullOrEmpty(userDto.Email))
                user = await _userRepository.GetByEmailAsync(userDto.Email);
            else if (!string.IsNullOrEmpty(userDto.UserName))
                user = await _userRepository.GetByUsernameAsync(userDto.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<List<User>> GetUsersAsync(string? username = null)
        {
            return await _userRepository.GetAllAsync(username);
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

        public async Task<string> DeleteUserAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return "Usuário não encontrado.";
            await _userRepository.DeleteAsync(user);
            return $"O email: {email} foi deletado com sucesso!";
        }
    }
}