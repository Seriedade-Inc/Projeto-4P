using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
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

            // // Mapeia para DTO
            // var blasphemyDtos = blasphemies.Select(b => new BlasphemyDto
            // {
            //     BlasphemyName = b.BlasphemyName,
            //     Fact = b.Fact,
            //     Passive = b.Passive,
            // }).ToList();

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

            // Mapeia para DTO
            // var blasphemyDto = new BlasphemyDto
            // {
            //     BlasphemyName = blasphemy.BlasphemyName,
            //     Fact = blasphemy.Fact,
            //     Passive = blasphemy.Passive,
            //     BlasphemyAbilities = blasphemy.BlasphemyAbilities?.Select(ba => new BlasphemyAbilitiesDto
            //     {
            //         AbilityName = ba.AbilityName,
            //         Description = ba.Description
            //     }).ToList()
            // };

            return Ok(blasphemy);
        }

        // POST: api/Blasphemy
        [HttpPost]
        public async Task<IActionResult> CreateBlasphemy([FromBody] BlasphemyDto blasphemyDto)
        {
            // Mapeia o DTO para a entidade
            var blasphemy = new Blasphemy
            {
                BlasphemyName = blasphemyDto.BlasphemyName,
                Fact = blasphemyDto.Fact,
                Passive = blasphemyDto.Passive,
            };

            _context.Set<Blasphemy>().Add(blasphemy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlasphemy), new { id = blasphemy.Id }, blasphemyDto);
        }

        // PUT: api/Blasphemy/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlasphemy(long id, [FromBody] BlasphemyDto blasphemyDto)
        {
            var existingBlasphemy = await _context.Set<Blasphemy>()
                .Include(b => b.BlasphemyAbilities)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existingBlasphemy == null)
                return NotFound("Blasphemy not found.");

            // Atualiza os valores da entidade com base no DTO
            existingBlasphemy.BlasphemyName = blasphemyDto.BlasphemyName;
            existingBlasphemy.Fact = blasphemyDto.Fact;
            existingBlasphemy.Passive = blasphemyDto.Passive;


            _context.Entry(existingBlasphemy).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Blasphemy/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlasphemy(long id)
        {
            var blasphemy = await _context.Set<Blasphemy>()
                .Include(b => b.BlasphemyAbilities)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blasphemy == null)
                return NotFound("Blasphemy not found.");

            _context.Set<BlasphemyAbilities>().RemoveRange(blasphemy.BlasphemyAbilities);
            _context.Set<Blasphemy>().Remove(blasphemy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}