using Projeto4pSharedLibrary.Classes;
using Projeto4pServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projeto4pServer.Repository
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetAllAsync();
        Task<Character?> GetByIdAsync(long id);
        Task<List<Character>> GetByUserIdAsync(Guid userId);
        Task<Character> CreateAsync(Character character);
        Task UpdateAsync(Character character);
    }

    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Character>> GetAllAsync()
        {
            return await _context.Characters
                .Include(c => c.Inventories)
                .Include(c => c.Blasphemies)
                .Include(c => c.Agendas)
                .Include(c => c.CharacterSkills)
                .ToListAsync();
        }

        public async Task<Character?> GetByIdAsync(long id)
        {
            return await _context.Characters
                .Include(c => c.Inventories)
                .Include(c => c.Blasphemies)
                .Include(c => c.Agendas)
                .Include(c => c.CharacterSkills)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Character>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Characters
                .Where(c => c.UserId == userId)
                .Include(c => c.Blasphemies)
                .Include(c => c.Agendas)
                .ToListAsync();
        }

        public async Task<Character> CreateAsync(Character character)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            return character;
        }

        public async Task UpdateAsync(Character character)
        {
            _context.Entry(character).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}