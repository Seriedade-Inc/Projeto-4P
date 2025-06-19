namespace Projeto4pServer.DTOs
{
    public class AgendaDto
    {
        public string AgendaName { get; set; } = string.Empty;
        public string NormalItem { get; set; } = string.Empty;
        public string BoldItem { get; set; } = string.Empty;
        public string SpecialRule { get; set; } = string.Empty;
        public string? Name { get; set; }
    // public List<AgendaAbilitiesDto>? Abilities { get; set; }
    }
}