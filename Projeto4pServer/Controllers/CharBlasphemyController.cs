using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
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
                .Include(cb => cb.Blasphemy)
                .Include(cb => cb.BlasphemyAbility)
                .ToListAsync();

            // Mapeia para DTO
            // var charBlasphemyDtos = charBlasphemies.Select(cb => new CharBlasphemyDto
            // {
            //     Blasphemy = cb.Blasphemy != null ? new BlasphemyDto
            //     {
            //         BlasphemyName = cb.Blasphemy.BlasphemyName,
            //         Fact = cb.Blasphemy.Fact,
            //         Passive = cb.Blasphemy.Passive
            //     } : null,
            //     BlasphemyAbility = cb.BlasphemyAbility != null ? new BlasphemyAbilitiesDto
            //     {
            //         AbilityName = cb.BlasphemyAbility.AbilityName,
            //         Description = cb.BlasphemyAbility.Description
            //     } : null
            // }).ToList();

            return Ok(charBlasphemies);
        }

        // GET: api/CharBlasphemy/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharBlasphemy(long id)
        {
            var charBlasphemy = await _context.CharBlasphemies
                .Include(cb => cb.Character)
                .Include(cb => cb.Blasphemy)
                .Include(cb => cb.BlasphemyAbility)
                .FirstOrDefaultAsync(cb => cb.Id == id);

            if (charBlasphemy == null)
                return NotFound("CharBlasphemy not found.");

            // Mapeia para DTO
            // var charBlasphemyDto = new CharBlasphemyDto
            // {
            //     Blasphemy = charBlasphemy.Blasphemy != null ? new BlasphemyDto
            //     {
            //         BlasphemyName = charBlasphemy.Blasphemy.BlasphemyName,
            //         Fact = charBlasphemy.Blasphemy.Fact,
            //         Passive = charBlasphemy.Blasphemy.Passive
            //     } : null,
            //     BlasphemyAbility = charBlasphemy.BlasphemyAbility != null ? new BlasphemyAbilitiesDto
            //     {
            //         AbilityName = charBlasphemy.BlasphemyAbility.AbilityName,
            //         Description = charBlasphemy.BlasphemyAbility.Description
            //     } : null
            // };

            return Ok(charBlasphemy);
        }

        // POST: api/CharBlasphemy
        [HttpPost]
        public async Task<IActionResult> CreateCharBlasphemy([FromBody] CharBlasphemyDto charBlasphemyDto)
        {
            // Valida se o personagem existe
            var character = await _context.Characters.FindAsync(charBlasphemyDto.CharacterId);
            if (character == null)
                return NotFound("Character not found.");

            if (charBlasphemyDto.BlasphemyId == null || 
                !await _context.Blasphemies.AnyAsync(b => b.Id == charBlasphemyDto.BlasphemyId))
            {
                return BadRequest("Invalid BlasphemyId. The blasphemy does not exist.");
            }

            // Verifica se o BlasphemyAbilityId é válido
            if (charBlasphemyDto.BlasphemyAbilityId == null || 
                !await _context.BlasphemyAbilities.AnyAsync(ba => ba.Id == charBlasphemyDto.BlasphemyAbilityId))
            {
                return BadRequest("Invalid BlasphemyAbilityId. The blasphemy ability does not exist.");
            }
                
            var charBlasphemy = new CharBlasphemy
            {
                BlasphemyId = charBlasphemyDto.BlasphemyId, // Conversão explícita
                BlasphemyAbilityId = charBlasphemyDto.BlasphemyAbilityId, // Conversão explícita
                CharacterId = charBlasphemyDto.CharacterId, // Conversão explícita
            };

            _context.CharBlasphemies.Add(charBlasphemy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharBlasphemy), new { id = charBlasphemy.Id }, charBlasphemyDto);
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