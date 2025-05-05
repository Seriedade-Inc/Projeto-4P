using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
    public class CharacterSkills
    { 
        public long Id { get; set; } // Chave primária
        public long CharacterId { get; set; } // Relacionamento com Character
        public int Force { get; set; } 
        public int Conditioning { get; set; } 
        public int Coordination { get; set; }  
        public int Covert { get; set; } 
        public int Interfacing { get; set; } 
        public int Investigation { get; set; } 
        public int Authority { get; set; } 
        public int Surveillance { get; set; } 
        public int Negotiation { get; set; }    
        public int Connection { get; set; }      
        [JsonIgnore]
        public Character? Character { get; set; } // Relacionamento com Character      
    }
}