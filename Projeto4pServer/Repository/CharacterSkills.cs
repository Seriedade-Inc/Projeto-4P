using Projeto4pSharedLibrary.Classes;
using Projeto4pServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Projeto4pServer.Repository
{
    public interface ICharacterSkillsRepository
    {
        Task<CharacterSkills?> GetByCharacterIdAsync(long characterId);
        Task<CharacterSkills> CreateAsync(CharacterSkills characterSkills);
        Task UpdateAsync(CharacterSkills characterSkills);
    }

    public class CharacterSkillsRepository : ICharacterSkillsRepository
    {
        private readonly AppDbContext _context;

        public CharacterSkillsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CharacterSkills?> GetByCharacterIdAsync(long characterId)
        {
            return await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterId);
        }

        public async Task<CharacterSkills> CreateAsync(CharacterSkills characterSkills)
        {
            _context.CharacterSkills.Add(characterSkills);
            await _context.SaveChangesAsync();
            return characterSkills;
        }

        public async Task UpdateAsync(CharacterSkills characterSkills)
        {
            _context.Entry(characterSkills).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}