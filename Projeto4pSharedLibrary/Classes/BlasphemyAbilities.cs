using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
    public class BlasphemyAbilities
    { 
        public long Id { get; set; } // Chave primária
        public long BlasphemyId { get; set; } // Chave estrangeira
        public string AbilityName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public Blasphemy? Blasphemy { get; set; } // Relacionamento 
    }
}