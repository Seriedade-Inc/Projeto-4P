using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
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

            return Ok(agenda);
        }

        // POST: api/Agenda
        [HttpPost]
        public async Task<IActionResult> CreateAgenda([FromBody] Agenda agenda)
        {
            if (agenda == null)
                return BadRequest("Agenda cannot be null.");

            _context.Set<Agenda>().Add(agenda);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAgenda), new { id = agenda.Id }, agenda);
        }

        // PUT: api/Agenda/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgenda(long id, [FromBody] Agenda agenda)
        {
            if (id != agenda.Id)
                return BadRequest("Agenda ID mismatch.");

            var existingAgenda = await _context.Set<Agenda>().FindAsync(id);
            if (existingAgenda == null)
                return NotFound("Agenda not found.");

            existingAgenda.AgendaName = agenda.AgendaName;
            existingAgenda.NormalItem = agenda.NormalItem;
            existingAgenda.BoldItem = agenda.BoldItem;
            existingAgenda.SpecialRule = agenda.SpecialRule;
            
            _context.Entry(existingAgenda).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Agenda/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgenda(long id)
        {
            var agenda = await _context.Set<Agenda>().FindAsync(id);
            if (agenda == null)
                return NotFound("Agenda not found.");

            _context.Set<Agenda>().Remove(agenda);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}