using System.Text.Json.Serialization;
namespace Projeto4pServer.DTOs
{
    public class AgendaDto
    {
        public long CharacterId { get; set; }
        public string AgendaName { get; set; } = string.Empty;
        public string AgendaText { get; set; } = string.Empty;
        [JsonIgnore]
        public string? Name { get; set; }
        // public List<AgendaAbilitiesDto>? Abilities { get; set; }
    }
}