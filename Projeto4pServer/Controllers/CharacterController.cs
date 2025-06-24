using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;
using System; // Para Guid
using System.Threading.Tasks; // Para Task
using Microsoft.AspNetCore.Http; // Para IFormFile

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

        
        [HttpGet("{userId:guid}")] 
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
                // O CharacterService agora aceita o IFormFile diretamente para processar a imagem
                var character = await _service.CreateCharacterAsync(userId, characterDto, Imagem);
                
                // Retorna 201 CreatedAtAction com o link para o novo recurso
                return CreatedAtAction(nameof(GetCharacters), new { id = character.Id }, character);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Usuário não encontrado, por exemplo
            }
            catch (ArgumentException ex) // Captura erros de validação da imagem do serviço (tipo/tamanho)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Logar o erro completo aqui
                Console.WriteLine($"Erro ao criar personagem: {ex.Message} - {ex.StackTrace}");
                return StatusCode(500, "Erro interno do servidor ao criar personagem.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacter(long id, [FromForm] UpdateCharacterDto updatedCharacterDto, IFormFile? Imagem = null)
        {
            try
            {
                // O CharacterService agora aceita o IFormFile diretamente para processar a imagem
                await _service.UpdateCharacterAsync(id, updatedCharacterDto, Imagem);
                return NoContent(); // 204 No Content para atualização bem-sucedida
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Personagem não encontrado
            }
            catch (ArgumentException ex) // Captura erros de validação da imagem do serviço (tipo/tamanho)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Logar o erro completo aqui
                Console.WriteLine($"Erro ao atualizar personagem: {ex.Message} - {ex.StackTrace}");
                return StatusCode(500, "Erro interno do servidor ao atualizar personagem.");
            }
        }

        // Deleta um personagem e sua imagem associada
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCharacter(long id)
        {
            try
            {
                // 1. Obter o personagem para ter a URL da imagem antes de deletá-lo do DB
                var characterToDelete = await _service.GetCharacterByIdAsync(id);
                if (characterToDelete == null)
                {
                    return NotFound($"Personagem com ID {id} não encontrado.");
                }

                // 2. Deletar o personagem do banco de dados (usando o método DeleteAsync do DeleteService base)
                await _service.DeleteAsync(id); 

                // 3. Deletar a imagem física do wwwroot
                if (!string.IsNullOrEmpty(characterToDelete.Imagem))
                {
                    _service.DeleteImageFromWWWRoot(characterToDelete.Imagem);
                }

                return Ok(new { message = "Character deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Personagem não encontrado
            }
            catch (Exception ex)
            {
                // Logar o erro completo aqui
                Console.WriteLine($"Erro ao deletar personagem: {ex.Message} - {ex.StackTrace}");
                return StatusCode(500, "Erro interno do servidor ao deletar personagem.");
            }
        }
    }
}