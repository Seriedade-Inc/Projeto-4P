using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
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

            // Mapeia para DTO
            // var characterSkillsDto = new CharacterSkillsDto
            // {
            //     Force = characterSkills.Force,
            //     Conditioning = characterSkills.Conditioning,
            //     Coordination = characterSkills.Coordination,
            //     Covert = characterSkills.Covert,
            //     Interfacing = characterSkills.Interfacing,
            //     Investigation = characterSkills.Investigation,
            //     Authority = characterSkills.Authority,
            //     Surveillance = characterSkills.Surveillance,
            //     Negotiation = characterSkills.Negotiation,
            //     Connection = characterSkills.Connection
            // };

            return Ok(characterSkills);
        }

        // POST: api/CharacterSkills
        [HttpPost]
        public async Task<IActionResult> CreateCharacterSkills(long characterId, [FromBody] CharacterSkillsDto characterSkillsDto)
        {
            var character = await _context.Characters.FindAsync(characterId);
            if (character == null)
                return NotFound("Character not found.");

            // Verifica se já existem habilidades para o personagem
            var existingSkills = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterId);

            if (existingSkills != null)
                return BadRequest("Character skills already exist.");

            // Mapeia o DTO para a entidade
            var characterSkills = new CharacterSkills
            {
                CharacterId = characterId,
                Force = characterSkillsDto.Force,
                Conditioning = characterSkillsDto.Conditioning,
                Coordination = characterSkillsDto.Coordination,
                Covert = characterSkillsDto.Covert,
                Interfacing = characterSkillsDto.Interfacing,
                Investigation = characterSkillsDto.Investigation,
                Authority = characterSkillsDto.Authority,
                Surveillance = characterSkillsDto.Surveillance,
                Negotiation = characterSkillsDto.Negotiation,
                Connection = characterSkillsDto.Connection
            };

            // Validação dos valores das habilidades
            if (!ValidateSkillValues(characterSkills))
                return BadRequest("Skill values must be between 0 and 4.");

            _context.CharacterSkills.Add(characterSkills);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharacterSkills), new { characterId = characterId }, characterSkillsDto);
        }

        // PUT: api/CharacterSkills/{characterId}
        [HttpPut("{characterId}")]
        public async Task<IActionResult> UpdateCharacterSkills(long characterId, [FromBody] CharacterSkillsDto updatedSkillsDto)
        {
            var characterSkills = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterId);

            if (characterSkills == null)
                return NotFound("Character skills not found.");

            // Validação dos valores das habilidades
            if (!ValidateSkillValues(new CharacterSkills
            {
                Force = updatedSkillsDto.Force,
                Conditioning = updatedSkillsDto.Conditioning,
                Coordination = updatedSkillsDto.Coordination,
                Covert = updatedSkillsDto.Covert,
                Interfacing = updatedSkillsDto.Interfacing,
                Investigation = updatedSkillsDto.Investigation,
                Authority = updatedSkillsDto.Authority,
                Surveillance = updatedSkillsDto.Surveillance,
                Negotiation = updatedSkillsDto.Negotiation,
                Connection = updatedSkillsDto.Connection
            }))
                return BadRequest("Skill values must be between 0 and 4.");

            // Atualiza os valores das habilidades
            characterSkills.Force = updatedSkillsDto.Force;
            characterSkills.Conditioning = updatedSkillsDto.Conditioning;
            characterSkills.Coordination = updatedSkillsDto.Coordination;
            characterSkills.Covert = updatedSkillsDto.Covert;
            characterSkills.Interfacing = updatedSkillsDto.Interfacing;
            characterSkills.Investigation = updatedSkillsDto.Investigation;
            characterSkills.Authority = updatedSkillsDto.Authority;
            characterSkills.Surveillance = updatedSkillsDto.Surveillance;
            characterSkills.Negotiation = updatedSkillsDto.Negotiation;
            characterSkills.Connection = updatedSkillsDto.Connection;

            await _context.SaveChangesAsync();
            return NoContent();
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