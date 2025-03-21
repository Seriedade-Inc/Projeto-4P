namespace Items;
public class Item{
    public required int ID { get; set; }
    public required string Name { get; set; }
    public int Value { get; set; }
    public int Armor { get; set; }
    public int UsesLeft { get; set; }
    public int MaxUses { get; set; }
    public bool isEquipped { get; set; }
    public bool isBulky { get; set; }
    public bool isPetty { get; set; }
}