using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
    public class CharBlasphemy
    {
        public long Id { get; set; } // Chave primária
        public long CharacterId { get; set; } // Relacionamento com Character
        public long BlasphemyAbilityId { get; set; } // Relacionamento com BlasphemyAbilities
        public long BlasphemyId { get; set; } // Relacionamento com Blasphemy

        
        public Blasphemy? Blasphemy { get; set; } // Relacionamento com BlasphemyAbilities
        [JsonIgnore]
        public Character? Character { get; set; } // Relacionamento com Character
        [JsonIgnore]
        public BlasphemyAbilities? BlasphemyAbility { get; set; } // Relacionamento com BlasphemyAbilities
    }
}