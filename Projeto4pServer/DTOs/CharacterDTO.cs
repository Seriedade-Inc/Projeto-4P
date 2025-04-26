public class CharacterDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<InventoryDto> Inventories { get; set; } = new List<InventoryDto>();
    
}