using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CharacterController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/User/Character
        [HttpGet]
        public IActionResult GetCharacters([FromQuery] long? id, [FromQuery] Guid? userId)
        {
            if (id.HasValue)
            {
                // Retorna o personagem específico pelo ID
                var character = _context.Characters
                .Include(c => c.Inventories)
                .Include(c => c.CharAgendas)
                    .ThenInclude(ca => ca.Agenda)
                .Include(c => c.CharAgendas)
                    .ThenInclude(ca => ca.AgendaAbility)
                .Include(c => c.CharBlasphemies)
                    .ThenInclude(cb => cb.Blasphemy)
                .Include(c => c.CharBlasphemies)
                    .ThenInclude(cb => cb.BlasphemyAbility)
                .Include(c => c.CharacterSkills)
                .FirstOrDefault(c => c.Id == id.Value);
                
                if (character == null)
                    return NotFound("Character not found.");

                return Ok(character);
            }

            if (userId.HasValue)
            {
                // Retorna todos os personagens associados ao usuário
                var userCharacters = _context.Characters.Where(c => c.UserId == userId.Value).ToList();
                if (!userCharacters.Any())
                    return NotFound("No characters found for this user.");

                return Ok(userCharacters);
            }

            // Retorna todos os personagens existentes
            var allCharacters = _context.Characters.ToList();
            return Ok(allCharacters);
        }
        // POST: api/User/Character
        [HttpPost("create")]
        public IActionResult CreateCharacter(Guid userId, [FromBody] CreateCharacterDto characterDto)
        {
            var user = _context.User.Find(userId);
            if (user == null)
                return NotFound("User not found.");

            // character.UserId = userId;
            // foreach (var inventory in character.Inventories)
            // {
            //     inventory.Character = null; // Remove a referência ao Character
            // }

            var character = new Character
            {   
                Name = characterDto.Name,
                Gender = characterDto.Gender,
                Heigth = characterDto.Heigth,
                Weigth = characterDto.Weigth,
                HairColor = characterDto.HairColor,
                EyeColor = characterDto.EyeColor,
                CAT = characterDto.CAT,
                DivineAgony = characterDto.DivineAgony,
                Stress = characterDto.Stress,
                Injury = characterDto.Injury,
                XP = characterDto.XP,
                Advance = characterDto.Advance,
                KitPoints = characterDto.KitPoints,
                Burst = characterDto.Burst,
                SinOverflow = characterDto.SinOverflow,
                Marks = characterDto.Marks
            };

            _context.Characters.Add(character);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCharacters), new {userId = userId, id = character.Id}, characterDto);
        }

        // PUT: api/User/Character/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateCharacter(Guid userId, long id, [FromBody] Character updatedCharacter)
        {
            var character = _context.Characters.FirstOrDefault(c => c.Id == id && c.UserId == userId);
            if (character == null)
                return NotFound("Character not found.");

            character.Name = updatedCharacter.Name;
            character.CharacterXID = updatedCharacter.CharacterXID;
            character.Agenda = updatedCharacter.Agenda;
            character.Blasfemia = updatedCharacter.Blasfemia;
            character.Gender = updatedCharacter.Gender;
            character.Heigth = updatedCharacter.Heigth;
            character.Weigth = updatedCharacter.Weigth;
            character.HairColor = updatedCharacter.HairColor;
            character.EyeColor = updatedCharacter.EyeColor;
            character.CAT = updatedCharacter.CAT;
            character.DivineAgony = updatedCharacter.DivineAgony;
            character.XP = updatedCharacter.XP;
            character.Advance = updatedCharacter.Advance;
            character.KitPoints = updatedCharacter.KitPoints;
            character.Inventories = updatedCharacter.Inventories;
            character.Burst = updatedCharacter.Burst;
            character.SinOverflow = updatedCharacter.SinOverflow;
            character.Marks = updatedCharacter.Marks;


            _context.SaveChanges();

            return Ok(character);
        }

        // DELETE: api/User/Character/{id}
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCharacter(long id)
        {
            var character = _context.Characters.FirstOrDefault(c => c.Id == id);
            if (character == null)
                return NotFound("Character not found.");

            _context.Characters.Remove(character);
            _context.SaveChanges();

            return Ok(new { message = "Character deleted successfully." });
        }
    }
}