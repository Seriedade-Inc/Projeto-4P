using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlasphemyAbilitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlasphemyAbilitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BlasphemyAbilities
        [HttpGet]
        public async Task<IActionResult> GetAbilities()
        {
            var abilities = await _context.Set<BlasphemyAbilities>()
                .Include(a => a.Blasphemy)
                .ToListAsync();
            return Ok(abilities);
        }

        // GET: api/BlasphemyAbilities/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAbility(long id)
        {
            var ability = await _context.Set<BlasphemyAbilities>()
                .Include(a => a.Blasphemy)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (ability == null)
                return NotFound("Ability not found.");

            return Ok(ability);
        }

        // POST: api/BlasphemyAbilities
        [HttpPost]
        public async Task<IActionResult> CreateAbility([FromBody] BlasphemyAbilities ability)
        {
            _context.Set<BlasphemyAbilities>().Add(ability);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAbility), new { id = ability.Id }, ability);
        }

        // PUT: api/BlasphemyAbilities/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbility(long id, [FromBody] BlasphemyAbilities ability)
        {
            if (id != ability.Id)
                return BadRequest("Ability ID mismatch.");

            var existingAbility = await _context.Set<BlasphemyAbilities>().FindAsync(id);
            if (existingAbility == null)
                return NotFound("Ability not found.");

            existingAbility.AbilityName = ability.AbilityName;
            existingAbility.Description = ability.Description;

            _context.Entry(existingAbility).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/BlasphemyAbilities/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbility(long id)
        {
            var ability = await _context.Set<BlasphemyAbilities>().FindAsync(id);
            if (ability == null)
                return NotFound("Ability not found.");

            _context.Set<BlasphemyAbilities>().Remove(ability);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}