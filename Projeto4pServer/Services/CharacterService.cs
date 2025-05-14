using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Services
{
    public class CharacterService : DeleteService<Character>
    {
        private readonly AppDbContext _context;

        public CharacterService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Character>> GetAllCharactersAsync()
        {
            return await _context.Characters
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
                .ToListAsync();
        }

        public async Task<Character?> GetCharacterByIdAsync(long id)
        {
            return await _context.Characters
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
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Character> CreateCharacterAsync(Guid userId, CreateCharacterDto characterDto)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var character = new Character
            {
                UserId = userId,
                CharacterXID = characterDto.CharacterXID,
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
            await _context.SaveChangesAsync();

            return character;
        }

        public async Task UpdateCharacterAsync(long id, UpdateCharacterDto updatedCharacter)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            if (character == null)
            {
                throw new KeyNotFoundException("Character not found.");
            }

            character.Name = updatedCharacter.Name;
            character.CharacterXID = updatedCharacter.CharacterXID;
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
            character.Burst = updatedCharacter.Burst;
            character.SinOverflow = updatedCharacter.SinOverflow;
            character.Marks = updatedCharacter.Marks;

            _context.Entry(character).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}