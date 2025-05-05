using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaAbilitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgendaAbilitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AgendaAbilities
        [HttpGet]
        public async Task<IActionResult> GetAbilities()
        {
            var abilities = await _context.Set<AgendaAbilities>()
                .Include(a => a.Agenda)
                .ToListAsync();
            return Ok(abilities);
        }

        // GET: api/AgendaAbilities/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAbility(long id)
        {
            var ability = await _context.Set<AgendaAbilities>()
                .Include(a => a.Agenda)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (ability == null)
                return NotFound("Ability not found.");

            return Ok(ability);
        }

        // POST: api/AgendaAbilities
        [HttpPost]
        public async Task<IActionResult> CreateAbility([FromBody] AgendaAbilities ability)
        {
            _context.Set<AgendaAbilities>().Add(ability);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAbility), new { id = ability.Id }, ability);
        }

        // PUT: api/AgendaAbilities/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbility(long id, [FromBody] AgendaAbilities ability)
        {
            if (id != ability.Id)
                return BadRequest("Ability ID mismatch.");

            var existingAbility = await _context.Set<AgendaAbilities>().FindAsync(id);
            if (existingAbility == null)
                return NotFound("Ability not found.");

            existingAbility.AbilityName = ability.AbilityName;
            existingAbility.Description = ability.Description;

            _context.Entry(existingAbility).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/AgendaAbilities/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbility(long id)
        {
            var ability = await _context.Set<AgendaAbilities>().FindAsync(id);
            if (ability == null)
                return NotFound("Ability not found.");

            _context.Set<AgendaAbilities>().Remove(ability);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}