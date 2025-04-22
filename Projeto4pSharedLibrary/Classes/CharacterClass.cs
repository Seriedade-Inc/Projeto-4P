using System.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace Projeto4pSharedLibrary.Classes
{
    public class Character
    {
        public long Id { get; set; } // Chave primária
        public Guid UserId { get; set; } // Relacionamento com o User
        public string Name { get; set; } = string.Empty;
        public string CharacterXID { get; set; } = string.Empty;
        public string Agenda { get; set; } = string.Empty;
        public string Blasfemia { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Heigth { get; set; } = string.Empty;
        public string Weigth { get; set; } = string.Empty;
        public string HairColor { get; set; } = string.Empty;
        public string EyeColor { get; set; } = string.Empty;
        public int CAT { get; set; } = 0;
        public int DivineAgony { get; set; } = 0;
        public int Injury { get; set; } = 0;
        public int Stress { get; set; } = 0;
        public int XP { get; set; } = 0;
        public int Advance { get; set; } = 0;
        public int KitPoints { get; set; } = 0;
        public int Burst { get; set; } = 0;
        public int SinOverflow { get; set;} = 0;
        public List<string> Marks { get; set; } = [];
        public Skills CharacterSkills { get; set; } = new Skills();
        public Inventory CharacterInventory { get; set; } = new Inventory();

        public class Inventory
        {
            [Key] // Mark CharacterID as the primary key
            public long CharacterID { get; set; }
            public List<string> Items { get; set; } = [];
        }

        public class Skills
        {
            [Key]
            public long CharacterID { get; set; }
            public int Force { get; set; } = 0;
            public int Conditioning { get; set; } = 0;
            public int Coordination { get; set; } = 0;
            public int Covert { get; set; } = 0;
            public int Interfacing { get; set; } = 0;
            public int Investigation { get; set; } = 0;
            public int Surveillance { get; set; } = 0;
            public int Negotiation { get; set; } = 0;
            public int Authority { get; set; } = 0;
            public int Connection { get; set; } = 0;
        }
    }
}