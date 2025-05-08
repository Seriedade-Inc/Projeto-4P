using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgendaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Agenda
        [HttpGet]
        public async Task<IActionResult> GetAgendas()
        {
            var agendas = await _context.Set<Agenda>()
                .Include(a => a.Abilities)
                .ToListAsync();

            // Mapeia para DTO
            // var agendaDtos = agendas.Select(a => new AgendaDto
            // {
            //     Id = a.Id,
            //     AgendaName = a.AgendaName,
            //     NormalItem = a.NormalItem,
            //     BoldItem = a.BoldItem,
            //     SpecialRule = a.SpecialRule
            // }).ToList();

            return Ok(agendas);
        }

        // GET: api/Agenda/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgenda(long id)
        {
            var agenda = await _context.Set<Agenda>()
                .Include(a => a.Abilities)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agenda == null)
                return NotFound("Agenda not found.");

            // // Mapeia para DTO
            // var agendaDto = new AgendaDto
            // {
            //     AgendaName = agenda.AgendaName,
            //     NormalItem = agenda.NormalItem,
            //     BoldItem = agenda.BoldItem,
            //     SpecialRule = agenda.SpecialRule
            // };

            return Ok(agenda);
        }

        // POST: api/Agenda
        [HttpPost]
        public async Task<IActionResult> CreateAgenda([FromBody] AgendaDto agendaDto)
        {
            if (string.IsNullOrWhiteSpace(agendaDto.AgendaName) ||
                string.IsNullOrWhiteSpace(agendaDto.NormalItem) ||
                string.IsNullOrWhiteSpace(agendaDto.BoldItem))
        {
            return BadRequest("All fields except 'SpecialRule' must be filled.");
        }

            // Mapeia o DTO para a entidade
            var agenda = new Agenda
            {
                AgendaName = agendaDto.AgendaName,
                NormalItem = agendaDto.NormalItem,
                BoldItem = agendaDto.BoldItem,
                SpecialRule = agendaDto.SpecialRule
            };

            _context.Set<Agenda>().Add(agenda);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgenda), new { id = agenda.Id }, new
            {
                agendaDto.AgendaName,
                agendaDto.NormalItem,
                agendaDto.BoldItem,
                agendaDto.SpecialRule
            });
        }

        // PUT: api/Agenda/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgenda(long id, [FromBody] AgendaDto agendaDto)
        {
            var existingAgenda = await _context.Set<Agenda>()
                .Include(a => a.Abilities)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingAgenda == null)
                return NotFound("Agenda not found.");

            // Atualiza os valores da entidade com base no DTO
            existingAgenda.AgendaName = agendaDto.AgendaName;
            existingAgenda.NormalItem = agendaDto.NormalItem;
            existingAgenda.BoldItem = agendaDto.BoldItem;
            existingAgenda.SpecialRule = agendaDto.SpecialRule;

            _context.Entry(existingAgenda).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Agenda/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgenda(long id)
        {
            var agenda = await _context.Set<Agenda>()
                .Include(a => a.Abilities)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agenda == null)
                return NotFound("Agenda not found.");

            _context.Set<AgendaAbilities>().RemoveRange(agenda.Abilities);
            _context.Set<Agenda>().Remove(agenda);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}