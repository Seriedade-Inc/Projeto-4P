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
                .ToListAsync();
        }

        public async Task<Agenda?> GetAgendaByIdAsync(long id)
        {
            return await _context.Set<Agenda>()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Agenda> CreateAgendaAsync(AgendaDto agendaDto)
        {
            if (string.IsNullOrWhiteSpace(agendaDto.AgendaName) ||
                string.IsNullOrWhiteSpace(agendaDto.AgendaText) ||
                agendaDto.CharacterId <= 0)
            {
                throw new ArgumentException("Todos os campos precisam ser preenchidos e o personagem tem que ser válido.");
            }

            var agenda = new Agenda
            {
                CharacterId = agendaDto.CharacterId,
                AgendaName = agendaDto.AgendaName,
                AgendaText = agendaDto.AgendaText
            };

            _context.Set<Agenda>().Add(agenda);
            await _context.SaveChangesAsync();

            return agenda;
        }

        public async Task UpdateAgendaAsync(long id, AgendaDto agendaDto)
        {
            var existingAgenda = await _context.Set<Agenda>()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingAgenda == null)
            {
                throw new KeyNotFoundException("Agenda not found.");
            }

            existingAgenda.CharacterId = agendaDto.CharacterId;
            existingAgenda.AgendaName = agendaDto.AgendaName;
            existingAgenda.AgendaText = agendaDto.AgendaText;

            _context.Entry(existingAgenda).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}