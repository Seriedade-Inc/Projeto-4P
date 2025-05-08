using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlasphemyAbilitiesController : ControllerBase
    {
        private readonly BlasphemyAbilitiesService _service;

        public BlasphemyAbilitiesController(BlasphemyAbilitiesService service)
        {
            _service = service;
        }

        // GET: api/BlasphemyAbilities
        [HttpGet]
        public async Task<IActionResult> GetAbilities()
        {
            var abilities = await _service.GetAllAbilitiesAsync();
            return Ok(abilities);
        }

        // GET: api/BlasphemyAbilities/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAbility(long id)
        {
            var ability = await _service.GetAbilityByIdAsync(id);
            if (ability == null)
                return NotFound("Ability not found.");

            return Ok(ability);
        }

        // POST: api/BlasphemyAbilities
        [HttpPost]
        public async Task<IActionResult> CreateAbility([FromBody] BlasphemyAbilitiesDto abilityDto)
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

        // PUT: api/BlasphemyAbilities/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbility(long id, [FromBody] BlasphemyAbilitiesDto abilityDto)
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

        // DELETE: api/BlasphemyAbilities/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbility(long id)
        {
            try
            {
                await _service.DeleteAsync(id); // Reutiliza o método genérico de delete do BaseService
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
