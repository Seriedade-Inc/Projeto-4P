// using Microsoft.AspNetCore.Mvc;
// using Projeto4pServer.Data;
// using Projeto4pSharedLibrary.Classes;

// namespace Projeto4pServer.Controllers
// {
//     [Route("api/User/{userId}/[controller]")]
//     [ApiController]
//     public class CharacterController : ControllerBase
//     {
//         private readonly AppDbContext _context;

//         public CharacterController(AppDbContext context)
//         {
//             _context = context;
//         }

//         // GET: api/User/{userId}/Character
//         [HttpGet]
//         public IActionResult GetCharacters(Guid userId)
//         {
//             var characters = _context.Characters.Where(c => c.UserId == userId).ToList();
//             if (!characters.Any())
//                 return NotFound("No characters found for this user.");

//             return Ok(characters);
//         }

//         // POST: api/User/{userId}/Character
//         [HttpPost]
//         public IActionResult CreateCharacter(Guid userId, [FromBody] Character character)
//         {
//             var user = _context.Users.Find(userId);
//             if (user == null)
//                 return NotFound("User not found.");

//             character.UserId = userId;
//             _context.Characters.Add(character);
//             _context.SaveChanges();

//             return CreatedAtAction(nameof(GetCharacters), new { userId = userId }, character);
//         }

//         // PUT: api/User/{userId}/Character/{id}
//         [HttpPut("{id}")]
//         public IActionResult UpdateCharacter(Guid userId, long id, [FromBody] Character updatedCharacter)
//         {
//             var character = _context.Characters.FirstOrDefault(c => c.Id == id && c.UserId == userId);
//             if (character == null)
//                 return NotFound("Character not found.");

//             character.Name = updatedCharacter.Name;
//             character.Background = updatedCharacter.Background;
//             character.Stats = updatedCharacter.Stats;
//             character.MaxStats = updatedCharacter.MaxStats;
//             character.HP = updatedCharacter.HP;
//             character.Armor = updatedCharacter.Armor;
//             character.Gold = updatedCharacter.Gold;
//             character.Traits = updatedCharacter.Traits;
//             character.Bonds = updatedCharacter.Bonds;
//             character.Omens = updatedCharacter.Omens;
//             character.Deprived = updatedCharacter.Deprived;

//             _context.SaveChanges();

//             return Ok(character);
//         }

//         // DELETE: api/User/{userId}/Character/{id}
//         [HttpDelete("{id}")]
//         public IActionResult DeleteCharacter(Guid userId, long id)
//         {
//             var character = _context.Characters.FirstOrDefault(c => c.Id == id && c.UserId == userId);
//             if (character == null)
//                 return NotFound("Character not found.");

//             _context.Characters.Remove(character);
//             _context.SaveChanges();

//             return Ok(new { message = "Character deleted successfully." });
//         }
//     }
// }