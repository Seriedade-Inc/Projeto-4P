using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Services
{
    public class CharBlasphemyService : DeleteService<CharBlasphemy>
    {
        private readonly AppDbContext _context;

        public CharBlasphemyService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CharBlasphemy>> GetAllCharBlasphemiesAsync()
        {
            return await _context.CharBlasphemies
                .Include(cb => cb.Character)
                .Include(cb => cb.Blasphemy)
                .Include(cb => cb.BlasphemyAbility)
                .ToListAsync();
        }

        public async Task<CharBlasphemy?> GetCharBlasphemyByIdAsync(long id)
        {
            return await _context.CharBlasphemies
                .Include(cb => cb.Character)
                .Include(cb => cb.Blasphemy)
                .Include(cb => cb.BlasphemyAbility)
                .FirstOrDefaultAsync(cb => cb.Id == id);
        }

        public async Task<CharBlasphemy> CreateCharBlasphemyAsync(CharBlasphemyDto charBlasphemyDto)
        {
            var character = await _context.Characters.FindAsync(charBlasphemyDto.CharacterId);
            if (character == null)
            {
                throw new KeyNotFoundException("Character not found.");
            }

            if (!await _context.Blasphemies.AnyAsync(b => b.Id == charBlasphemyDto.BlasphemyId))
            {
                throw new ArgumentException("Invalid BlasphemyId. The blasphemy does not exist.");
            }

            if (!await _context.BlasphemyAbilities.AnyAsync(ba => ba.Id == charBlasphemyDto.BlasphemyAbilityId))
            {
                throw new ArgumentException("Invalid BlasphemyAbilityId. The blasphemy ability does not exist.");
            }

            var charBlasphemy = new CharBlasphemy
            {
                BlasphemyId = charBlasphemyDto.BlasphemyId,
                BlasphemyAbilityId = charBlasphemyDto.BlasphemyAbilityId,
                CharacterId = charBlasphemyDto.CharacterId
            };

            _context.CharBlasphemies.Add(charBlasphemy);
            await _context.SaveChangesAsync();

            return charBlasphemy;
        }
    }
}