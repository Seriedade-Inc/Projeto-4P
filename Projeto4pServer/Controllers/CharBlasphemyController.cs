using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharBlasphemyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CharBlasphemyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CharBlasphemy
        [HttpGet]
        public async Task<IActionResult> GetCharBlasphemies()
        {
            var charBlasphemies = await _context.CharBlasphemies
                .Include(cb => cb.Character)
                .Include(cb => cb.BlasphemyAbility)
                .Include(cb => cb.Blasphemy)
                .ToListAsync();
            return Ok(charBlasphemies);
        }

        // GET: api/CharBlasphemy/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharBlasphemy(long id)
        {
            var charBlasphemy = await _context.CharBlasphemies
                .Include(cb => cb.Character)
                .Include(cb => cb.BlasphemyAbility)
                .Include(cb => cb.Blasphemy)
                .FirstOrDefaultAsync(cb => cb.Id == id);

            if (charBlasphemy == null)
                return NotFound("CharBlasphemy not found.");

            return Ok(charBlasphemy);
        }

        // POST: api/CharBlasphemy
        [HttpPost]
        public async Task<IActionResult> CreateCharBlasphemy([FromBody] CharBlasphemy charBlasphemy)
        {
            var character = await _context.Characters.FindAsync(charBlasphemy.CharacterId);
            if (character == null)
                return NotFound("Character not found.");

            _context.CharBlasphemies.Add(charBlasphemy);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCharBlasphemy), new { id = charBlasphemy.Id }, charBlasphemy);
        }

        // DELETE: api/CharBlasphemy/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharBlasphemy(long id)
        {
            var charBlasphemy = await _context.CharBlasphemies.FindAsync(id);
            if (charBlasphemy == null)
                return NotFound("CharBlasphemy not found.");

            _context.CharBlasphemies.Remove(charBlasphemy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}