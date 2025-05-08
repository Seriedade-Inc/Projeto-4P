using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Services
{
    public class CharacterSkillsService : DeleteService<CharacterSkills>
    {
        private readonly AppDbContext _context;

        public CharacterSkillsService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CharacterSkills?> GetCharacterSkillsAsync(long characterId)
        {
            return await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterId);
        }

        public async Task<CharacterSkills> CreateCharacterSkillsAsync(long characterId, CharacterSkillsDto skillsDto)
        {
            var character = await _context.Characters.FindAsync(characterId);
            if (character == null)
            {
                throw new KeyNotFoundException("Character not found.");
            }

            var existingSkills = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterId);

            if (existingSkills != null)
            {
                throw new ArgumentException("Character skills already exist.");
            }

            var characterSkills = new CharacterSkills
            {
                CharacterId = characterId,
                Force = skillsDto.Force,
                Conditioning = skillsDto.Conditioning,
                Coordination = skillsDto.Coordination,
                Covert = skillsDto.Covert,
                Interfacing = skillsDto.Interfacing,
                Investigation = skillsDto.Investigation,
                Authority = skillsDto.Authority,
                Surveillance = skillsDto.Surveillance,
                Negotiation = skillsDto.Negotiation,
                Connection = skillsDto.Connection
            };

            if (!ValidateSkillValues(characterSkills))
            {
                throw new ArgumentException("Skill values must be between 0 and 4.");
            }

            _context.CharacterSkills.Add(characterSkills);
            await _context.SaveChangesAsync();

            return characterSkills;
        }

        public async Task UpdateCharacterSkillsAsync(long characterId, CharacterSkillsDto updatedSkillsDto)
        {
            var characterSkills = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterId);

            if (characterSkills == null)
            {
                throw new KeyNotFoundException("Character skills not found.");
            }

            var updatedSkills = new CharacterSkills
            {
                Force = updatedSkillsDto.Force,
                Conditioning = updatedSkillsDto.Conditioning,
                Coordination = updatedSkillsDto.Coordination,
                Covert = updatedSkillsDto.Covert,
                Interfacing = updatedSkillsDto.Interfacing,
                Investigation = updatedSkillsDto.Investigation,
                Authority = updatedSkillsDto.Authority,
                Surveillance = updatedSkillsDto.Surveillance,
                Negotiation = updatedSkillsDto.Negotiation,
                Connection = updatedSkillsDto.Connection
            };

            if (!ValidateSkillValues(updatedSkills))
            {
                throw new ArgumentException("Skill values must be between 0 and 4.");
            }

            characterSkills.Force = updatedSkillsDto.Force;
            characterSkills.Conditioning = updatedSkillsDto.Conditioning;
            characterSkills.Coordination = updatedSkillsDto.Coordination;
            characterSkills.Covert = updatedSkillsDto.Covert;
            characterSkills.Interfacing = updatedSkillsDto.Interfacing;
            characterSkills.Investigation = updatedSkillsDto.Investigation;
            characterSkills.Authority = updatedSkillsDto.Authority;
            characterSkills.Surveillance = updatedSkillsDto.Surveillance;
            characterSkills.Negotiation = updatedSkillsDto.Negotiation;
            characterSkills.Connection = updatedSkillsDto.Connection;

            await _context.SaveChangesAsync();
        }

        private bool ValidateSkillValues(CharacterSkills skills)
        {
            return skills.Force >= 0 && skills.Force <= 4 &&
                   skills.Conditioning >= 0 && skills.Conditioning <= 4 &&
                   skills.Coordination >= 0 && skills.Coordination <= 4 &&
                   skills.Covert >= 0 && skills.Covert <= 4 &&
                   skills.Interfacing >= 0 && skills.Interfacing <= 4 &&
                   skills.Investigation >= 0 && skills.Investigation <= 4 &&
                   skills.Authority >= 0 && skills.Authority <= 4 &&
                   skills.Surveillance >= 0 && skills.Surveillance <= 4 &&
                   skills.Negotiation >= 0 && skills.Negotiation <= 4 &&
                   skills.Connection >= 0 && skills.Connection <= 4;
        }
    }
}