using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Repository;

namespace Projeto4pServer.Services
{
    public class AgendaService : DeleteService<Agenda>
    {
        private readonly IAgendaRepository _repository;

        public AgendaService(IAgendaRepository repository, AppDbContext context) : base(context)
        {
            _repository = repository;
        }

        public async Task<List<Agenda>> GetAllAgendasAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Agenda?> GetAgendaByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
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

            return await _repository.CreateAsync(agenda);
        }

        public async Task UpdateAgendaAsync(long id, AgendaDto agendaDto)
        {
            var existingAgenda = await _repository.GetByIdAsync(id);

            if (existingAgenda == null)
            {
                throw new KeyNotFoundException("Agenda not found.");
            }

            existingAgenda.CharacterId = agendaDto.CharacterId;
            existingAgenda.AgendaName = agendaDto.AgendaName;
            existingAgenda.AgendaText = agendaDto.AgendaText;

            await _repository.UpdateAsync(existingAgenda);
        }

        // Antes da refatoração (dentro de AgendaService):
        // public async Task DeleteAgendaAsync(long id)
        // {
        //     var agendaToDelete = await _context.Set<Agenda>().FindAsync(id);
        //     if (agendaToDelete != null)
        //     {
        //         _context.Set<Agenda>().Remove(agendaToDelete);
        //         await _context.SaveChangesAsync();
        //     }
        // }
        // Como o delete era genérico, agora usamos a classe base DeleteService:

    }
}