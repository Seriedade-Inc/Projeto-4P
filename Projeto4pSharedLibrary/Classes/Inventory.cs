using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
    public class Inventory
    {
        public long Id { get; set; } // Chave primária
        public long CharacterId { get; set; } // Chave estrangeira para Character
        public string ItemName { get; set; } = string.Empty; // Nome do item
        public int Quantity { get; set; } // Quantidade do item

        // Relacionamento com Character
        [JsonIgnore]
        public Character? Character { get; set; }
    }
}