namespace Projeto4pSharedLibrary.Classes
{
  public class Agenda
  {
    public long Id { get; set; } // Chave primária
    public string Name { get; set; } = string.Empty;
    public string Items { get; set; } = string.Empty;
    public string SpecialRule { get; set; } = string.Empty;

    public List<AgendaAbilities> Abilities { get; set; } = new List<AgendaAbilities>();
  }

}