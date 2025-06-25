using Projeto4pServer.Data;
using Projeto4pSharedLibrary.Classes;
using Microsoft.EntityFrameworkCore;

namespace Projeto4pServer.Repository
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<List<User>> GetAllAsync(string? username = null);
        Task CreateAsync(User user);
        Task DeleteAsync(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) { _context = context; }

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.User.AnyAsync(u => u.Email == email);

        public async Task<bool> UsernameExistsAsync(string username) =>
            await _context.User.AnyAsync(u => u.UserName.ToLower() == username.ToLower());

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.User.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _context.User.FirstOrDefaultAsync(u => u.UserName == username);

        public async Task<List<User>> GetAllAsync(string? username = null)
        {
            var query = _context.User.Include(u => u.Characters).AsQueryable();
            if (!string.IsNullOrEmpty(username))
                query = query.Where(u => u.UserName.Contains(username));
            return await query.OrderBy(u => u.UserName).ToListAsync();
        }

        public async Task CreateAsync(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
