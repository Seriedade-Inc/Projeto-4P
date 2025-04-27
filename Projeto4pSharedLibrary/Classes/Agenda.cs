using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
  public class Agenda
  {
    public long Id { get; set; } // Chave primária
    public string AgendaName { get; set; } = string.Empty;
    public string NormalItem { get; set; } = string.Empty;
    public string BoldItem { get; set; } = string.Empty;
    public string SpecialRule { get; set; } = string.Empty;

    [JsonIgnore]
    public List<AgendaAbilities> Abilities { get; set; } = new List<AgendaAbilities>();
  }

}