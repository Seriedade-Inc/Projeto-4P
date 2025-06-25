using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;
using Projeto4pServer.Repository;

namespace Projeto4pServer.Services
{
    public class CharacterSkillsService : DeleteService<CharacterSkills>
    {
        private readonly ICharacterSkillsRepository _repository;
        private readonly AppDbContext _context;

        public CharacterSkillsService(ICharacterSkillsRepository repository, AppDbContext context) : base(context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<CharacterSkills?> GetCharacterSkillsAsync(long characterId)
        {
            return await _repository.GetByCharacterIdAsync(characterId);
        }

        public async Task<CharacterSkills> CreateCharacterSkillsAsync(long characterId, CharacterSkillsDto skillsDto)
        {
            var character = await _context.Characters.FindAsync(characterId);
            if (character == null)
                throw new KeyNotFoundException("Character not found.");

            var existingSkills = await _repository.GetByCharacterIdAsync(characterId);
            if (existingSkills != null)
                throw new ArgumentException("Character skills already exist.");

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
                throw new ArgumentException("Skill values must be between 0 and 4.");

            return await _repository.CreateAsync(characterSkills);
        }

        public async Task UpdateCharacterSkillsAsync(long characterId, CharacterSkillsDto updatedSkillsDto)
        {
            var characterSkills = await _repository.GetByCharacterIdAsync(characterId);
            if (characterSkills == null)
                throw new KeyNotFoundException("Character skills not found.");

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

            if (!ValidateSkillValues(characterSkills))
                throw new ArgumentException("Skill values must be between 0 and 4.");

            await _repository.UpdateAsync(characterSkills);
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