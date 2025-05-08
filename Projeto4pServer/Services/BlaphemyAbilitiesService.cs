using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Services
{
    public class BlasphemyAbilitiesService : DeleteService<BlasphemyAbilities>
    {
        private readonly AppDbContext _context;

        public BlasphemyAbilitiesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BlasphemyAbilities>> GetAllAbilitiesAsync()
        {
            return await _context.Set<BlasphemyAbilities>()
                .Include(a => a.Blasphemy)
                .ToListAsync();
        }

        public async Task<BlasphemyAbilities?> GetAbilityByIdAsync(long id)
        {
            return await _context.Set<BlasphemyAbilities>()
                .Include(a => a.Blasphemy)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<BlasphemyAbilities> CreateAbilityAsync(BlasphemyAbilitiesDto abilityDto)
        {
            if (string.IsNullOrWhiteSpace(abilityDto.AbilityName) ||
                string.IsNullOrWhiteSpace(abilityDto.Description))
            {
                throw new ArgumentException("All fields must be filled.");
            }

            if (!await _context.Blasphemies.AnyAsync(b => b.Id == abilityDto.BlasphemyId))
            {
                throw new ArgumentException("Invalid BlasphemyId. The blasphemy does not exist.");
            }

            var ability = new BlasphemyAbilities
            {
                BlasphemyId = abilityDto.BlasphemyId,
                AbilityName = abilityDto.AbilityName,
                Description = abilityDto.Description
            };

            _context.Set<BlasphemyAbilities>().Add(ability);
            await _context.SaveChangesAsync();

            return ability;
        }

        public async Task UpdateAbilityAsync(long id, BlasphemyAbilitiesDto abilityDto)
        {
            var existingAbility = await _context.Set<BlasphemyAbilities>().FindAsync(id);
            if (existingAbility == null)
            {
                throw new KeyNotFoundException("Ability not found.");
            }

            if (string.IsNullOrWhiteSpace(abilityDto.AbilityName) ||
                string.IsNullOrWhiteSpace(abilityDto.Description))
            {
                throw new ArgumentException("All fields must be filled.");
            }

            if (!await _context.Blasphemies.AnyAsync(b => b.Id == abilityDto.BlasphemyId))
            {
                throw new ArgumentException("Invalid BlasphemyId. The blasphemy does not exist.");
            }

            existingAbility.BlasphemyId = abilityDto.BlasphemyId;
            existingAbility.AbilityName = abilityDto.AbilityName;
            existingAbility.Description = abilityDto.Description;

            _context.Entry(existingAbility).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}