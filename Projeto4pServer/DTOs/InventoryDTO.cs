namespace Projeto4pServer.DTOs
{
public class InventoryDto
{
    public long CharacterId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
}