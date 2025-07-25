using System.Text.Json.Serialization;

namespace Projeto4pSharedLibrary.Classes
{
    public class Character
    {   
        public long Id { get; set; } // Chave primária
        public Guid UserId { get; set; } // Relacionamento com o User
        public string Imagem { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CharacterXID { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string HairColor { get; set; } = string.Empty;
        public string EyeColor { get; set; } = string.Empty;
        public int CAT { get; set; }
        public int DivineAgony { get; set; }
        public int Stress { get; set; }
        public int Injury { get; set; }
        public int XP { get; set; }
        public int Advance { get; set; }
        public int KitPoints { get; set; }
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();
        public List<Blasphemy> Blasphemies { get; set; } = new List<Blasphemy>();
        public List<Agenda> Agendas { get; set; } = new List<Agenda>();
        public CharacterSkills? CharacterSkills { get; set; }
        public int Burst { get; set; }
        public int SinOverflow { get; set;}
        public int Marks { get; set; } 

    }
}