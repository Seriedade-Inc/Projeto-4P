using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharAgendaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CharAgendaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CharAgenda
        [HttpGet]
        public async Task<IActionResult> GetCharAgendas()
        {
            var CharAgendas = await _context.CharAgendas
                .Include(ca => ca.Character)
                .Include(ca => ca.AgendaAbility)
                .Include(ca => ca.Agenda)
                .ToListAsync();
            return Ok(CharAgendas);
        }

        // GET: api/CharAgenda/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharAgenda(long id)
        {
            var charAgenda = await _context.CharAgendas
                .Include(ca => ca.Character)
                .Include(ca => ca.AgendaAbility)
                .Include(ca => ca.Agenda)
                .FirstOrDefaultAsync(ca => ca.Id == id);

            if (charAgenda == null)
                return NotFound("CharAgenda not found.");

            return Ok(charAgenda);
        }

        // POST: api/CharAgenda
        [HttpPost]
        public async Task<IActionResult> CreateCharAgenda([FromBody] CharAgenda charAgenda)
        {
            var character = await _context.Characters.FindAsync(charAgenda.CharacterId);
            if (character == null)
                return NotFound("Character not found.");

            _context.CharAgendas.Add(charAgenda);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCharAgenda), new { id = charAgenda.Id }, charAgenda);
        }

        // DELETE: api/CharAgenda/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharAgenda(long id)
        {
            var charAgenda = await _context.CharAgendas.FindAsync(id);
            if (charAgenda == null)
                return NotFound("CharAgenda not found.");

            _context.CharAgendas.Remove(charAgenda);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}