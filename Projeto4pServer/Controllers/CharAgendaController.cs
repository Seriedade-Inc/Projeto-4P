using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharAgendaController : ControllerBase
    {
        private readonly CharAgendaService _service;

        public CharAgendaController(CharAgendaService service)
        {
            _service = service;
        }

        // GET: api/CharAgenda
        [HttpGet]
        public async Task<IActionResult> GetCharAgendas()
        {
            var charAgendas = await _service.GetAllCharAgendasAsync();
            return Ok(charAgendas);
        }

        // GET: api/CharAgenda/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharAgenda(long id)
        {
            var charAgenda = await _service.GetCharAgendaByIdAsync(id);
            if (charAgenda == null)
                return NotFound("CharAgenda not found.");

            return Ok(charAgenda);
        }

        // POST: api/CharAgenda
        [HttpPost]
        public async Task<IActionResult> CreateCharAgenda([FromBody] CharAgendaDto charAgendaDto)
        {
            try
            {
                var charAgenda = await _service.CreateCharAgendaAsync(charAgendaDto);
                return CreatedAtAction(nameof(GetCharAgenda), new { id = charAgenda.Id }, charAgenda);
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

        // DELETE: api/CharAgenda/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharAgenda(long id)
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