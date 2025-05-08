using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Services
{
    public class AgendaAbilitiesService : DeleteService<AgendaAbilities>
    {
        private readonly AppDbContext _context;

        public AgendaAbilitiesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AgendaAbilities>> GetAllAbilitiesAsync()
        {
            return await _context.Set<AgendaAbilities>()
                .Include(a => a.Agenda)
                .ToListAsync();
        }

        public async Task<AgendaAbilities?> GetAbilityByIdAsync(long id)
        {
            return await _context.Set<AgendaAbilities>()
                .Include(a => a.Agenda)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AgendaAbilities> CreateAbilityAsync(AgendaAbilitiesDto abilityDto)
        {
            if (string.IsNullOrWhiteSpace(abilityDto.AbilityName) ||
                string.IsNullOrWhiteSpace(abilityDto.Description))
            {
                throw new ArgumentException("All fields must be filled.");
            }

            if (!await _context.Agendas.AnyAsync(a => a.Id == abilityDto.AgendaId))
            {
                throw new ArgumentException("Invalid AgendaId. The agenda does not exist.");
            }

            var ability = new AgendaAbilities
            {
                AgendaId = abilityDto.AgendaId,
                AbilityName = abilityDto.AbilityName,
                Description = abilityDto.Description
            };

            _context.Set<AgendaAbilities>().Add(ability);
            await _context.SaveChangesAsync();

            return ability;
        }

        public async Task UpdateAbilityAsync(long id, AgendaAbilitiesDto abilityDto)
        {
            var existingAbility = await _context.Set<AgendaAbilities>().FindAsync(id);
            if (existingAbility == null)
            {
                throw new KeyNotFoundException("Ability not found.");
            }

            if (string.IsNullOrWhiteSpace(abilityDto.AbilityName) ||
                string.IsNullOrWhiteSpace(abilityDto.Description))
            {
                throw new ArgumentException("All fields must be filled.");
            }

            if (!await _context.Agendas.AnyAsync(a => a.Id == abilityDto.AgendaId))
            {
                throw new ArgumentException("Invalid AgendaId. The agenda does not exist.");
            }

            existingAbility.AgendaId = abilityDto.AgendaId;
            existingAbility.AbilityName = abilityDto.AbilityName;
            existingAbility.Description = abilityDto.Description;

            _context.Entry(existingAbility).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}