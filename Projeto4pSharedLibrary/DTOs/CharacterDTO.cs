namespace Projeto4pServer.DTOs
{  
public class CreateCharacterDto
{
    public Guid UserId { get; set; }
    public string? Imagem { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CharacterXID { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Heigth { get; set; } = string.Empty;
    public string Weigth { get; set; } = string.Empty;
    public string HairColor { get; set; } = string.Empty;
    public string EyeColor { get; set; } = string.Empty;
    public int CAT { get; set; }
    public int DivineAgony { get; set; } 
    public int Stress { get; set; } 
    public int Injury { get; set; }
    public int XP { get; set; } 
    public int Advance { get; set; }
    public int KitPoints { get; set; } = 5;
    public int Burst { get; set; }
    public int SinOverflow { get; set; }
    public int Marks { get; set; }
}

// DTO para PUT
public class UpdateCharacterDto
{
    public string? Imagem { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CharacterXID { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Heigth { get; set; } = string.Empty;
    public string Weigth { get; set; } = string.Empty;
    public string HairColor { get; set; } = string.Empty;
    public string EyeColor { get; set; } = string.Empty;
    public int CAT { get; set; }
    public int DivineAgony { get; set; }
    public int Stress { get; set; }
    public int Injury { get; set; }
    public int XP { get; set; }
    public int Advance { get; set; }
    public int KitPoints { get; set; }
    public int Burst { get; set; }
    public int SinOverflow { get; set; }
    public int Marks { get; set; }
}

// DTO para GET
public class CharacterDto
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public string Imagem { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CharacterXID { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Heigth { get; set; } = string.Empty;
    public string Weigth { get; set; } = string.Empty;
    public string HairColor { get; set; } = string.Empty;
    public string EyeColor { get; set; } = string.Empty;
    public int CAT { get; set; }
    public int DivineAgony { get; set; }
    public int Stress { get; set; }
    public int Injury { get; set; }
    public int XP { get; set; }
    public int Advance { get; set; }
    public int KitPoints { get; set; }
    public List<InventoryDto> Inventories { get; set; } = new List<InventoryDto>();
    public CharacterSkillsDto? CharacterSkills { get; set; }
    public List<AgendaDto>? Agendas { get; set; }
    public List<BlasphemyDto>? Blasphemies { get; set; }
    public int Burst { get; set; }
    public int SinOverflow { get; set; }
    public int Marks { get; set; } 
}
}