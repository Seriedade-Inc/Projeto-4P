namespace Projeto4pSharedLibrary.Classes
{
    public class User
    {
        
        public Guid Id { get; set; }
        public string UserName { get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<Character> Characters { get; set; } = new List<Character>();
        public string? ResetCode { get; set; } = string.Empty; // * Pensar em fazer no front, definir ainda
        public DateTime? ResetCodeExpiration { get; set; } // * Pensar em fazer no front, definir ainda
    }
}

//     public class Character{
//         public long Id { get; set; }
//         public Guid UserId { get; set; }
//         public string Name { get; set; } = string.Empty;
//         public string Background { get; set; } = string.Empty;
//         public Dictionary<string, int> Stats { get; set; } = [];
//         public Dictionary<string, int> MaxStats { get; set; } = [];
//         public Dictionary<int, int> HP { get; set; } = [];
//         public int? Armor { get; set; }
//         public int? Gold { get; set; }
//         public Dictionary<string, string> Traits = [];
//         public Dictionary<string, string> Bonds = [];
//         public Dictionary<string, string> Omens = [];
//         public bool Deprived { get; set; } = false;
//         public DateTime CreatedAt { get; set; } = DateTime.Now;

//         public Inventory? Inventory { get; set; }
//     }

//     public class Inventory{
//         public Guid UserId { get; set; }
//         public List<Item> Items { get; set; } = [];
//     }
// }