using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
    public class CharAgenda
    {
        public long Id { get; set; } // Chave primária
        public long CharacterId { get; set; } // Relacionamento com Character
        public long AgendaAbilityId { get; set; } // Relacionamento com AgendaAbilities
        public long AgendaId { get; set; } // Relacionamento com Agenda
       
        public Agenda? Agenda { get; set; }
        [JsonIgnore]
        public Character? Character { get; set; } // Relacionamento com Character
        [JsonIgnore]
        public AgendaAbilities? AgendaAbility { get; set; } // Relacionamento com AgendaAbilities
    }
}