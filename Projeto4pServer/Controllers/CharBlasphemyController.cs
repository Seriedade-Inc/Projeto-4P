using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharBlasphemyController : ControllerBase
    {
        private readonly CharBlasphemyService _service;

        public CharBlasphemyController(CharBlasphemyService service)
        {
            _service = service;
        }

        // GET: api/CharBlasphemy
        [HttpGet]
        public async Task<IActionResult> GetCharBlasphemies()
        {
            var charBlasphemies = await _service.GetAllCharBlasphemiesAsync();
            return Ok(charBlasphemies);
        }

        // GET: api/CharBlasphemy/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharBlasphemy(long id)
        {
            var charBlasphemy = await _service.GetCharBlasphemyByIdAsync(id);
            if (charBlasphemy == null)
                return NotFound("CharBlasphemy not found.");

            return Ok(charBlasphemy);
        }

        // POST: api/CharBlasphemy
        [HttpPost]
        public async Task<IActionResult> CreateCharBlasphemy([FromBody] CharBlasphemyDto charBlasphemyDto)
        {
            try
            {
                var charBlasphemy = await _service.CreateCharBlasphemyAsync(charBlasphemyDto);
                return CreatedAtAction(nameof(GetCharBlasphemy), new { id = charBlasphemy.Id }, charBlasphemy);
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

        // DELETE: api/CharBlasphemy/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharBlasphemy(long id)
        {
            try
            {
                await _service.DeleteAsync(id); // Reutiliza o método genérico de delete do DeleteService
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}