using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;

namespace Projeto4pServer.Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly CharacterService _service;

        public CharacterController(CharacterService service)
        {
            _service = service;
        }

        // GET: api/User/Character
        [HttpGet]
        public async Task<IActionResult> GetCharacters([FromQuery] long? id)
        {
            if (id.HasValue)
            {
                var character = await _service.GetCharacterByIdAsync(id.Value);
                if (character == null)
                    return NotFound("Character not found.");

                return Ok(character);
            }

            var characters = await _service.GetAllCharactersAsync();
            return Ok(characters);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCharactersByUserId(Guid userId)
        {
            var characters = await _service.GetCharactersByUserIdAsync(userId);
            if (characters == null || !characters.Any())
            {
                return NotFound("No characters found for this user.");
            }

            return Ok(characters);
        }

        // POST: api/User/Character/create
        [HttpPost("create/{userId}")]
        public async Task<IActionResult> CreateCharacter(Guid userId, [FromBody] CreateCharacterDto characterDto)
        {
            try
            {
                var character = await _service.CreateCharacterAsync(userId, characterDto);
                return CreatedAtAction(nameof(GetCharacters), new { id = character.Id }, character);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/User/Character/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacter(long id, [FromBody] UpdateCharacterDto updatedCharacter)
        {
            try
            {
                await _service.UpdateCharacterAsync(id, updatedCharacter);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/User/Character/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCharacter(long id)
        {
            try
            {
                await _service.DeleteAsync(id); // Reutiliza o método genérico de delete do DeleteService
                return Ok(new { message = "Character deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}