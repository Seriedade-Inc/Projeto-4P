namespace Projeto4pServer.DTOs{
  public class CharBlasphemyDto
  {
    public long CharacterId { get; set; }
    public long BlasphemyId { get; set; }
    public long BlasphemyAbilityId { get; set; }
  
    public BlasphemyDto? Blasphemy { get; set; }
}
}