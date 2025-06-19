using System.Text.Json.Serialization;
namespace Projeto4pServer.DTOs
{
  public class BlasphemyDto
  {
    public string BlasphemyName { get; set; } = string.Empty;
    public string Fact { get; set; } = string.Empty;
    public string Passive { get; set; } = string.Empty;
    [JsonIgnore]
    public string? Name { get; set; }
  }
}