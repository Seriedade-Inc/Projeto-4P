using System.Text.Json.Serialization;
namespace Projeto4pServer.DTOs
{
  public class BlasphemyDto
  {
    public long CharacterId { get; set; }
    public string BlasphemyName { get; set; } = string.Empty;
    public string BlasphemyText { get; set; } = string.Empty;
    [JsonIgnore]
    public string? Name { get; set; }
  }
}