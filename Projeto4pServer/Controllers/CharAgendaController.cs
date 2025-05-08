using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharAgendaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CharAgendaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CharAgenda
        [HttpGet]
        public async Task<IActionResult> GetCharAgendas()
        {
            var CharAgendas = await _context.CharAgendas
                .Include(ca => ca.Character)
                .Include(ca => ca.Agenda)
                .Include(ca => ca.AgendaAbility)
                .ToListAsync();

            // // Mapeia para DTO
            // var charAgendaDtos = charAgendas.Select(ca => new CharAgendaDto
            // {
            //     Agenda = ca.Agenda != null ? new AgendaDto
            //     {
            //         AgendaName = ca.Agenda.AgendaName,
            //         NormalItem = ca.Agenda.NormalItem,
            //         BoldItem = ca.Agenda.BoldItem,
            //         SpecialRule = ca.Agenda.SpecialRule
            //     } : null,
            //     AgendaAbility = ca.AgendaAbility != null ? new AgendaAbilitiesDto
            //     {
            //         AbilityName = ca.AgendaAbility.AbilityName,
            //         Description = ca.AgendaAbility.Description
            //     } : null
            // }).ToList();

            return Ok(CharAgendas);
        }

        // GET: api/CharAgenda/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharAgenda(long id)
        {
            var charAgenda = await _context.CharAgendas
                .Include(ca => ca.Character)
                .Include(ca => ca.AgendaAbility)
                .Include(ca => ca.Agenda)
                .FirstOrDefaultAsync(ca => ca.Id == id);

            if (charAgenda == null)
                return NotFound("CharAgenda not found.");

            // Mapeia para DTO
            // var charAgendaDto = new CharAgendaDto
            // {
            //     Agenda = charAgenda.Agenda != null ? new AgendaDto
            //     {
            //         AgendaName = charAgenda.Agenda.AgendaName,
            //         NormalItem = charAgenda.Agenda.NormalItem,
            //         BoldItem = charAgenda.Agenda.BoldItem,
            //         SpecialRule = charAgenda.Agenda.SpecialRule
            //     } : null,
            //     AgendaAbility = charAgenda.AgendaAbility != null ? new AgendaAbilitiesDto
            //     {
            //         AbilityName = charAgenda.AgendaAbility.AbilityName,
            //         Description = charAgenda.AgendaAbility.Description
            //     } : null
            // };

            return Ok(charAgenda);
        }

        // POST: api/CharAgenda
        [HttpPost]
        public async Task<IActionResult> CreateCharAgenda([FromBody] CharAgendaDto charAgendaDto)
        {
            // Valida se o personagem existe
            var character = await _context.Characters.FindAsync(charAgendaDto.CharacterId);
            if (character == null)
                return NotFound("Character not found.");

            if (charAgendaDto.AgendaId == null || 
                !await _context.Agendas.AnyAsync(a => a.Id == charAgendaDto.AgendaId))
            {
                return BadRequest("Invalid AgendaId. The agenda does not exist.");
            }

            // Verifica se o AgendaAbilityId é válido
            if (charAgendaDto.AgendaAbilityId == null || 
                !await _context.AgendaAbilities.AnyAsync(aa => aa.Id == charAgendaDto.AgendaAbilityId))
            {   
                return BadRequest("Invalid AgendaAbilityId. The agenda ability does not exist.");
            }

            var charAgenda = new CharAgenda
            {
                AgendaId = charAgendaDto.AgendaId, // Conversão explícita
                AgendaAbilityId = charAgendaDto.AgendaAbilityId, // Conversão explícita
                CharacterId = charAgendaDto.CharacterId
            };

            _context.CharAgendas.Add(charAgenda);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharAgenda), new { id = charAgenda.Id }, charAgendaDto);
        }

        // DELETE: api/CharAgenda/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharAgenda(long id)
        {
            var charAgenda = await _context.CharAgendas.FindAsync(id);
            if (charAgenda == null)
                return NotFound("CharAgenda not found.");

            _context.CharAgendas.Remove(charAgenda);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}