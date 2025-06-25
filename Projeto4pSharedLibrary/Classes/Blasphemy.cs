using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
    public class Blasphemy
    {
        public long Id { get; set; } // Chave primária
        public long CharacterId { get; set; }
        public string BlasphemyName { get; set; } = string.Empty;
        public string BlasphemyText { get; set; } = string.Empty;
        
        public Character? Character { get; set; }
    }
}