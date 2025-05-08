using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly AgendaService _service;

        public AgendaController(AgendaService service)
        {
            _service = service;
        }

        // GET: api/Agenda
        [HttpGet]
        public async Task<IActionResult> GetAgendas()
        {
            var agendas = await _service.GetAllAgendasAsync();
            return Ok(agendas);
        }

        // GET: api/Agenda/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgenda(long id)
        {
            var agenda = await _service.GetAgendaByIdAsync(id);
            if (agenda == null)
                return NotFound("Agenda not found.");

            return Ok(agenda);
        }

        // POST: api/Agenda
        [HttpPost]
        public async Task<IActionResult> CreateAgenda([FromBody] AgendaDto agendaDto)
        {
            try
            {
                var agenda = await _service.CreateAgendaAsync(agendaDto);
                return CreatedAtAction(nameof(GetAgenda), new { id = agenda.Id }, agenda);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Agenda/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgenda(long id, [FromBody] AgendaDto agendaDto)
        {
            try
            {
                await _service.UpdateAgendaAsync(id, agendaDto);
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

        // DELETE: api/Agenda/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgenda(long id)
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