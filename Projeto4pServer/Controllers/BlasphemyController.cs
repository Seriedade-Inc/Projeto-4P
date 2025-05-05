using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlasphemyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlasphemyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Blasphemy
        [HttpGet]
        public async Task<IActionResult> GetBlasphemies()
        {
            var blasphemies = await _context.Set<Blasphemy>()
                .Include(b => b.BlasphemyAbilities)
                .ToListAsync();
            return Ok(blasphemies);
        }

        // GET: api/Blasphemy/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlasphemy(long id)
        {
            var blasphemy = await _context.Set<Blasphemy>()
                .Include(b => b.BlasphemyAbilities)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blasphemy == null)
                return NotFound("Blasphemy not found.");

            return Ok(blasphemy);
        }

        // POST: api/Blasphemy
        [HttpPost]
        public async Task<IActionResult> CreateBlasphemy([FromBody] Blasphemy blasphemy)
        {
            _context.Set<Blasphemy>().Add(blasphemy);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBlasphemy), new { id = blasphemy.Id }, blasphemy);
        }

        // PUT: api/Blasphemy/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlasphemy(long id, [FromBody] Blasphemy blasphemy)
        {
            if (id != blasphemy.Id)
                return BadRequest("Blasphemy ID mismatch.");

            var existingBlasphemy = await _context.Set<Blasphemy>().FindAsync(id);
            if (existingBlasphemy == null)
                return NotFound("Blasphemy not found.");

            existingBlasphemy.BlasphemyName = blasphemy.BlasphemyName;
            existingBlasphemy.Fact = blasphemy.Fact;
            existingBlasphemy.Passive = blasphemy.Passive;

            _context.Entry(existingBlasphemy).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Blasphemy/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlasphemy(long id)
        {
            var blasphemy = await _context.Set<Blasphemy>().FindAsync(id);
            if (blasphemy == null)
                return NotFound("Blasphemy not found.");

            _context.Set<Blasphemy>().Remove(blasphemy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}