using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterSkillsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CharacterSkillsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CharacterSkills/{characterId}
        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetCharacterSkills(long characterId)
        {
            var characterSkills = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterId);

            if (characterSkills == null)
                return NotFound("Character skills not found.");

            return Ok(characterSkills);
        }

        // POST: api/CharacterSkills
        [HttpPost]
        public async Task<IActionResult> CreateCharacterSkills([FromBody] CharacterSkills characterSkills)
        {
            var character = await _context.Characters.FindAsync(characterSkills.CharacterId);
            if (character == null)
                return NotFound("Character not found.");

            // Verifica se já existem habilidades para o personagem
            var existingSkills = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterSkills.CharacterId);

            if (existingSkills != null)
                return BadRequest("Character skills already exist.");

            // Validação dos valores das habilidades
            if (!ValidateSkillValues(characterSkills))
                return BadRequest("Skill values must be between 0 and 4.");

            _context.CharacterSkills.Add(characterSkills);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharacterSkills), new { characterId = characterSkills.CharacterId }, characterSkills);
        }

        // PUT: api/CharacterSkills/{characterId}
        [HttpPut("{characterId}")]
        public async Task<IActionResult> UpdateCharacterSkills(long characterId, [FromBody] CharacterSkills updatedSkills)
        {
            var characterSkills = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterId);

            if (characterSkills == null)
                return NotFound("Character skills not found.");

            // Validação dos valores das habilidades
            if (!ValidateSkillValues(updatedSkills))
                return BadRequest("Skill values must be between 0 and 4.");

            // Atualiza os valores das habilidades
            characterSkills.Force = updatedSkills.Force;
            characterSkills.Conditioning = updatedSkills.Conditioning;
            characterSkills.Coordination = updatedSkills.Coordination;
            characterSkills.Covert = updatedSkills.Covert;
            characterSkills.Interfacing = updatedSkills.Interfacing;
            characterSkills.Investigation = updatedSkills.Investigation;
            characterSkills.Authority = updatedSkills.Authority;
            characterSkills.Surveillance = updatedSkills.Surveillance;
            characterSkills.Negotiation = updatedSkills.Negotiation;
            characterSkills.Connection = updatedSkills.Connection;

            await _context.SaveChangesAsync();
            return Ok("Character skills updated successfully.");
        }

        // DELETE: api/CharacterSkills/{characterId}
        [HttpDelete("{characterId}")]
        public async Task<IActionResult> DeleteCharacterSkills(long characterId)
        {
            var characterSkills = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterId);

            if (characterSkills == null)
                return NotFound("Character skills not found.");

            _context.CharacterSkills.Remove(characterSkills);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Validação dos valores das habilidades
        private bool ValidateSkillValues(CharacterSkills skills)
        {
            return skills.Force >= 0 && skills.Force <= 4 &&
                   skills.Conditioning >= 0 && skills.Conditioning <= 4 &&
                   skills.Coordination >= 0 && skills.Coordination <= 4 &&
                   skills.Covert >= 0 && skills.Covert <= 4 &&
                   skills.Interfacing >= 0 && skills.Interfacing <= 4 &&
                   skills.Investigation >= 0 && skills.Investigation <= 4 &&
                   skills.Authority >= 0 && skills.Authority <= 4 &&
                   skills.Surveillance >= 0 && skills.Surveillance <= 4 &&
                   skills.Negotiation >= 0 && skills.Negotiation <= 4 &&
                   skills.Connection >= 0 && skills.Connection <= 4;
        }
    }
}