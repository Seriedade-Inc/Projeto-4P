namespace Items;

public class Item
{
    private int ID { get; set; }

    public required string Name { get; set; }
    public int Value { get; set; }
    public int Armor { get; set; }

    public bool IsEquipped { get; set; }

    public bool IsBulky { get; set; }

    public bool IsPetty { get; set; }

    public int UsesLeft { get; set; }

    public int MaxUses { get; set; }
}