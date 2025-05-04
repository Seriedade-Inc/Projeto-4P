using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
    public class Blasphemy
    { 
        public long Id { get; set; } // Chave primária
        public string BlasphemyName { get; set; } = string.Empty;
        public string Fact { get; set; } = string.Empty;
        public string Passive { get; set; } = string.Empty;

        [JsonIgnore]
        public List<BlasphemyAbilities> BlasphemyAbilities { get; set; } = new List<BlasphemyAbilities>();
    }
}