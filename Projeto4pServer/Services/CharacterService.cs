using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Projeto4pServer.Services
{
    public class CharacterService : DeleteService<Character>
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CharacterService(AppDbContext context, IWebHostEnvironment environment) : base(context)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<Character>> GetAllCharactersAsync() =>
            await _context.Characters
                .Include(c => c.Inventories)
                .Include(c => c.CharAgendas).ThenInclude(ca => ca.Agenda)
                .Include(c => c.CharAgendas).ThenInclude(ca => ca.AgendaAbility)
                .Include(c => c.CharBlasphemies).ThenInclude(cb => cb.Blasphemy)
                .Include(c => c.CharBlasphemies).ThenInclude(cb => cb.BlasphemyAbility)
                .Include(c => c.CharacterSkills)
                .ToListAsync();

        public async Task<Character?> GetCharacterByIdAsync(long id) =>
            await _context.Characters
                .Include(c => c.Inventories)
                .Include(c => c.CharAgendas).ThenInclude(ca => ca.Agenda)
                .Include(c => c.CharAgendas).ThenInclude(ca => ca.AgendaAbility)
                .Include(c => c.CharBlasphemies).ThenInclude(cb => cb.Blasphemy)
                .Include(c => c.CharBlasphemies).ThenInclude(cb => cb.BlasphemyAbility)
                .Include(c => c.CharacterSkills)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<List<CharacterDto>> GetCharactersByUserIdAsync(Guid userId) =>
            (await _context.Characters
                .Where(c => c.UserId == userId)
                .Include(c => c.CharBlasphemies).ThenInclude(cb => cb.Blasphemy)
                .Include(c => c.CharAgendas).ThenInclude(ca => ca.Agenda)
                .ToListAsync())
            .Select(c => new CharacterDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Imagem = c.Imagem,
                Name = c.Name,
                CharacterXID = c.CharacterXID,
                Gender = c.Gender,
                Heigth = c.Height,
                Weigth = c.Weight,
                HairColor = c.HairColor,
                EyeColor = c.EyeColor,
                CAT = c.CAT,
                DivineAgony = c.DivineAgony,
                Stress = c.Stress,
                Injury = c.Injury,
                XP = c.XP,
                Advance = c.Advance,
                KitPoints = c.KitPoints,
                Burst = c.Burst,
                SinOverflow = c.SinOverflow,
                Marks = c.Marks,
                CharBlasphemies = c.CharBlasphemies.Select(cb => new CharBlasphemyDto { Blasphemy = new BlasphemyDto { Name = cb.Blasphemy?.BlasphemyName } }).ToList(),
                CharAgendas = c.CharAgendas.Select(ca => new CharAgendaDto { Agenda = new AgendaDto { Name = ca.Agenda?.AgendaName } }).ToList()
            }).ToList();

        public async Task<Character> CreateCharacterAsync(Guid userId, CreateCharacterDto characterDto, IFormFile? Imagem = null)
        {
            var imagemUrl = string.IsNullOrWhiteSpace(characterDto.Imagem) ? "/images/default.png" : characterDto.Imagem ?? string.Empty;

            if (Imagem != null)
            {
                imagemUrl = await SaveImageAndGetUrlAsync(Imagem);
            }

            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            
            var character = new Character
            {
                UserId = userId,
                Imagem = imagemUrl,
                Name = characterDto.Name,
                CharacterXID = characterDto.CharacterXID,
                Gender = characterDto.Gender,
                Height = characterDto.Heigth,
                Weight = characterDto.Weigth,
                HairColor = characterDto.HairColor,
                EyeColor = characterDto.EyeColor,
                CAT = characterDto.CAT,
                DivineAgony = characterDto.DivineAgony,
                Stress = characterDto.Stress,
                Injury = characterDto.Injury,
                XP = characterDto.XP,
                Advance = characterDto.Advance,
                KitPoints = characterDto.KitPoints,
                Burst = characterDto.Burst,
                SinOverflow = characterDto.SinOverflow,
                Marks = characterDto.Marks
            };
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            return character;
        }

        public async Task UpdateCharacterAsync(long id, UpdateCharacterDto updatedCharacter, IFormFile? Imagem = null)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            if (character == null)
            {
                throw new KeyNotFoundException("Character not found.");
            }

            if (Imagem != null)
            {
                // **O Imagem é o IFormFile, aqui ele é passado para o método que salva o arquivo**
                character.Imagem = await SaveImageAndGetUrlAsync(Imagem);
            }
            else if (!string.IsNullOrWhiteSpace(updatedCharacter.Imagem))
            {
                 character.Imagem = updatedCharacter.Imagem;
            }
            else
            {
                character.Imagem = "/images/default.png";
            }

            character.Name = updatedCharacter.Name;
            character.CharacterXID = updatedCharacter.CharacterXID;
            character.Gender = updatedCharacter.Gender;
            character.Height = updatedCharacter.Heigth;
            character.Weight = updatedCharacter.Weigth;
            character.HairColor = updatedCharacter.HairColor;
            character.EyeColor = updatedCharacter.EyeColor;
            character.CAT = updatedCharacter.CAT;
            character.DivineAgony = updatedCharacter.DivineAgony;
            character.XP = updatedCharacter.XP;
            character.Advance = updatedCharacter.Advance;
            character.KitPoints = updatedCharacter.KitPoints;
            character.Burst = updatedCharacter.Burst;
            character.SinOverflow = updatedCharacter.SinOverflow;
            character.Marks = updatedCharacter.Marks;

            _context.Entry(character).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private async Task<string> SaveImageAndGetUrlAsync(IFormFile Imagem)
        {
            // Validação 1: O arquivo existe e tem conteúdo?
            if (Imagem == null || Imagem.Length == 0)
            {
                throw new ArgumentException("Nenhum arquivo de imagem foi enviado ou o arquivo está vazio.");
            }

            // Validação 2: Tipo de arquivo
            if (!Imagem.ContentType.StartsWith("image/"))
            {
                throw new ArgumentException("Formato de arquivo inválido. Apenas imagens são permitidas.");
            }

            // Validação 3: Tamanho do arquivo
            var maxFileSize = 5 * 1024 * 1024; // 5MB
            if (Imagem.Length > maxFileSize)
            {
                throw new ArgumentException($"O arquivo é muito grande. Tamanho máximo permitido: {maxFileSize / (1024 * 1024)} MB.");
            }

            // **IMPORTANTE**: Validação para Imagem.FileName
            // Garante que o nome do arquivo não é nulo ou vazio antes de usar Path.GetExtension
            if (string.IsNullOrWhiteSpace(Imagem.FileName))
            {
                throw new ArgumentException("Nome do arquivo de imagem inválido ou ausente.");
            }

            string wwwRootPath = _environment.WebRootPath;
            string imagesFolder = Path.Combine(wwwRootPath, "images"); // wwwRootPath NÃO deve ser nulo aqui se a app.UseStaticFiles() estiver configurada.

            // Garante que a pasta 'images' exista
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Gera um nome de arquivo único para evitar colisões
            string fileExtension = Path.GetExtension(Imagem.FileName);
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            string fullPath = Path.Combine(imagesFolder, uniqueFileName); // imagesFolder e uniqueFileName NÃO devem ser nulos aqui.

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await Imagem.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }

        public void DeleteImageFromWWWRoot(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl) || imageUrl.Equals("/images/default.png", StringComparison.OrdinalIgnoreCase)) return;
            string fullPath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
                try { System.IO.File.Delete(fullPath); }
                catch (IOException ex) { Console.WriteLine($"Could not delete file {fullPath}: {ex.Message}"); }
        }
    }
}