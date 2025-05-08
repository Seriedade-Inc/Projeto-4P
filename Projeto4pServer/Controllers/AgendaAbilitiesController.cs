using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaAbilitiesController : ControllerBase
    {
        private readonly AgendaAbilitiesService _service;

        public AgendaAbilitiesController(AgendaAbilitiesService service)
        {
            _service = service;
        }

        // GET: api/AgendaAbilities
        [HttpGet]
        public async Task<IActionResult> GetAbilities()
        {
            var abilities = await _service.GetAllAbilitiesAsync();
            return Ok(abilities);
        }

        // GET: api/AgendaAbilities/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAbility(long id)
        {
            var ability = await _service.GetAbilityByIdAsync(id);
            if (ability == null)
                return NotFound("Ability not found.");

            return Ok(ability);
        }

        // POST: api/AgendaAbilities
        [HttpPost]
        public async Task<IActionResult> CreateAbility([FromBody] AgendaAbilitiesDto abilityDto)
        {
            try
            {
                var ability = await _service.CreateAbilityAsync(abilityDto);
                return CreatedAtAction(nameof(GetAbility), new { id = ability.Id }, ability);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/AgendaAbilities/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbility(long id, [FromBody] AgendaAbilitiesDto abilityDto)
        {
            try
            {
                await _service.UpdateAbilityAsync(id, abilityDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/AgendaAbilities/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbility(long id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}