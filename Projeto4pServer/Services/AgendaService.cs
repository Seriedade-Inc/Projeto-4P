using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;
using Microsoft.EntityFrameworkCore;

namespace Projeto4pServer.Services
{
    public class AgendaService : DeleteService<Agenda>
    {
        private readonly AppDbContext _context;

        public AgendaService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Agenda>> GetAllAgendasAsync()
        {
            return await _context.Set<Agenda>()
                .Include(a => a.Abilities)
                .ToListAsync();
        }

        public async Task<Agenda?> GetAgendaByIdAsync(long id)
        {
            return await _context.Set<Agenda>()
                .Include(a => a.Abilities)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Agenda> CreateAgendaAsync(AgendaDto agendaDto)
        {
            if (string.IsNullOrWhiteSpace(agendaDto.AgendaName) ||
                string.IsNullOrWhiteSpace(agendaDto.NormalItem) ||
                string.IsNullOrWhiteSpace(agendaDto.BoldItem))
            {
                throw new ArgumentException("All fields except 'SpecialRule' must be filled.");
            }

            var agenda = new Agenda
            {
                AgendaName = agendaDto.AgendaName,
                NormalItem = agendaDto.NormalItem,
                BoldItem = agendaDto.BoldItem,
                SpecialRule = agendaDto.SpecialRule
            };

            _context.Set<Agenda>().Add(agenda);
            await _context.SaveChangesAsync();

            return agenda;
        }

        public async Task UpdateAgendaAsync(long id, AgendaDto agendaDto)
        {
            var existingAgenda = await _context.Set<Agenda>()
                .Include(a => a.Abilities)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingAgenda == null)
            {
                throw new KeyNotFoundException("Agenda not found.");
            }

            existingAgenda.AgendaName = agendaDto.AgendaName;
            existingAgenda.NormalItem = agendaDto.NormalItem;
            existingAgenda.BoldItem = agendaDto.BoldItem;
            existingAgenda.SpecialRule = agendaDto.SpecialRule;

            _context.Entry(existingAgenda).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}