using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Services
{
    public class CharAgendaService : DeleteService<CharAgenda>
    {
        private readonly AppDbContext _context;

        public CharAgendaService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CharAgenda>> GetAllCharAgendasAsync()
        {
            return await _context.CharAgendas
                .Include(ca => ca.Character)
                .Include(ca => ca.Agenda)
                .Include(ca => ca.AgendaAbility)
                .ToListAsync();
        }

        public async Task<CharAgenda?> GetCharAgendaByIdAsync(long id)
        {
            return await _context.CharAgendas
                .Include(ca => ca.Character)
                .Include(ca => ca.Agenda)
                .Include(ca => ca.AgendaAbility)
                .FirstOrDefaultAsync(ca => ca.Id == id);
        }

        public async Task<CharAgenda> CreateCharAgendaAsync(CharAgendaDto charAgendaDto)
        {
            var character = await _context.Characters.FindAsync(charAgendaDto.CharacterId);
            if (character == null)
            {
                throw new KeyNotFoundException("Character not found.");
            }

            if (!await _context.Agendas.AnyAsync(a => a.Id == charAgendaDto.AgendaId))
            {
                throw new ArgumentException("Invalid AgendaId. The agenda does not exist.");
            }

            if (!await _context.AgendaAbilities.AnyAsync(aa => aa.Id == charAgendaDto.AgendaAbilityId))
            {
                throw new ArgumentException("Invalid AgendaAbilityId. The agenda ability does not exist.");
            }

            var charAgenda = new CharAgenda
            {
                AgendaId = charAgendaDto.AgendaId,
                AgendaAbilityId = charAgendaDto.AgendaAbilityId,
                CharacterId = charAgendaDto.CharacterId
            };

            _context.CharAgendas.Add(charAgenda);
            await _context.SaveChangesAsync();

            return charAgenda;
        }
    }
}