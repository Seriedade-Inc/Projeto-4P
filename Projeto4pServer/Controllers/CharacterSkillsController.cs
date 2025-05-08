using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterSkillsController : ControllerBase
    {
        private readonly CharacterSkillsService _service;

        public CharacterSkillsController(CharacterSkillsService service)
        {
            _service = service;
        }

        // GET: api/CharacterSkills/{characterId}
        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetCharacterSkills(long characterId)
        {
            var characterSkills = await _service.GetCharacterSkillsAsync(characterId);
            if (characterSkills == null)
                return NotFound("Character skills not found.");

            return Ok(characterSkills);
        }

        // POST: api/CharacterSkills
        [HttpPost]
        public async Task<IActionResult> CreateCharacterSkills(long characterId, [FromBody] CharacterSkillsDto characterSkillsDto)
        {
            try
            {
                var characterSkills = await _service.CreateCharacterSkillsAsync(characterId, characterSkillsDto);
                return CreatedAtAction(nameof(GetCharacterSkills), new { characterId = characterId }, characterSkillsDto);
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

        // PUT: api/CharacterSkills/{characterId}
        [HttpPut("{characterId}")]
        public async Task<IActionResult> UpdateCharacterSkills(long characterId, [FromBody] CharacterSkillsDto updatedSkillsDto)
        {
            try
            {
                await _service.UpdateCharacterSkillsAsync(characterId, updatedSkillsDto);
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

        // DELETE: api/CharacterSkills/{characterId}
        [HttpDelete("{characterId}")]
        public async Task<IActionResult> DeleteCharacterSkills(long characterId)
        {
            try
            {
                await _service.DeleteAsync(characterId); // Reutiliza o método genérico de delete do DeleteService
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}