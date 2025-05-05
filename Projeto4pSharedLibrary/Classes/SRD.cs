using System.ComponentModel;

namespace Projeto4pSharedLibrary.Classes
{
    public class Item{
        public int Id { get; set; }
        public List<ItemTypes> Types { get; set; } = [];
        public string Name { get; set; } = string.Empty;
        
    }
    public class ItemTypes{
        public string Name { get; set; } = string.Empty;
    }
    
}