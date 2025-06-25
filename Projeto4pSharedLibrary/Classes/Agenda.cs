using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
  public class Agenda
  {
    public long Id { get; set; } // Chave primária
    public long CharacterId { get; set; } // Chave estrangeira para o Character
    public string AgendaName { get; set; } = string.Empty;
    public string AgendaText { get; set; } = string.Empty;
    public Character? Character { get; set; }
  }
}