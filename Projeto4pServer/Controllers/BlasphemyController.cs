using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlasphemyController : ControllerBase
    {
        private readonly BlasphemyService _service;

        public BlasphemyController(BlasphemyService service)
        {
            _service = service;
        }

        // GET: api/Blasphemy
        [HttpGet]
        public async Task<IActionResult> GetBlasphemies()
        {
            var blasphemies = await _service.GetAllBlasphemiesAsync();
            return Ok(blasphemies);
        }

        // GET: api/Blasphemy/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlasphemy(long id)
        {
            var blasphemy = await _service.GetBlasphemyByIdAsync(id);
            if (blasphemy == null)
                return NotFound("Blasphemy not found.");

            return Ok(blasphemy);
        }

        // POST: api/Blasphemy
        [HttpPost]
        public async Task<IActionResult> CreateBlasphemy([FromBody] BlasphemyDto blasphemyDto)
        {
            var blasphemy = await _service.CreateBlasphemyAsync(blasphemyDto);
            return CreatedAtAction(nameof(GetBlasphemy), new { id = blasphemy.Id }, blasphemy);
        }

        // PUT: api/Blasphemy/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlasphemy(long id, [FromBody] BlasphemyDto blasphemyDto)
        {
            try
            {
                await _service.UpdateBlasphemyAsync(id, blasphemyDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Blasphemy/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlasphemy(long id)
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