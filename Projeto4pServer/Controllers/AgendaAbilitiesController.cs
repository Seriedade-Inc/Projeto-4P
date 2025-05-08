using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
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

            // // Mapeia para DTO
            // var abilitiesDto = abilities.Select(a => new AgendaAbilitiesDto
            // {
            //     AbilityName = a.AbilityName,
            //     Description = a.Description
            // }).ToList();

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

            // // Mapeia para DTO
            // var abilityDto = new AgendaAbilitiesDto
            // {
            //     AbilityName = ability.AbilityName,
            //     Description = ability.Description
            // };

            return Ok(ability);
        }

        // POST: api/AgendaAbilities
        [HttpPost]
        public async Task<IActionResult> CreateAbility([FromBody] AgendaAbilitiesDto abilityDto)
        {
            if (string.IsNullOrWhiteSpace(abilityDto.AbilityName) ||
                string.IsNullOrWhiteSpace(abilityDto.Description))
            {
                return BadRequest("All fields except must be filled.");
            }

            if (abilityDto.AgendaId == null || 
                !await _context.Agendas.AnyAsync(a => a.Id == abilityDto.AgendaId))
            {
                return BadRequest("Invalid AgendaId. The agenda does not exist.");
            }

            // Mapeia o DTO para a entidade
            var ability = new AgendaAbilities
            {   
                AgendaId = abilityDto.AgendaId,
                AbilityName = abilityDto.AbilityName,
                Description = abilityDto.Description
            };

            _context.Set<AgendaAbilities>().Add(ability);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAbility), new { id = ability.Id }, abilityDto);
        }

        // PUT: api/AgendaAbilities/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbility(long id, [FromBody] AgendaAbilitiesDto abilityDto)
        {
            var existingAbility = await _context.Set<AgendaAbilities>().FindAsync(id);
            if (existingAbility == null)
                return NotFound("Ability not found.");

            if (string.IsNullOrWhiteSpace(abilityDto.AbilityName) ||
                string.IsNullOrWhiteSpace(abilityDto.Description))
            {
                return BadRequest("All fields except must be filled.");
            }

            if (abilityDto.AgendaId == null || 
                !await _context.Agendas.AnyAsync(a => a.Id == abilityDto.AgendaId))
            {
                return BadRequest("Invalid AgendaId. The agenda does not exist.");
            }
            // Atualiza os valores da entidade com base no DTO
            existingAbility.AgendaId = abilityDto.AgendaId;
            existingAbility.AbilityName = abilityDto.AbilityName;
            existingAbility.Description = abilityDto.Description;

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