using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using Projeto4pServer.Repository;
// ...outros usings...

namespace Projeto4pServer.Services
{
    public class CharacterService : DeleteService<Character>
    {
        private readonly ICharacterRepository _repository;
        private readonly IWebHostEnvironment _environment;

        public CharacterService(ICharacterRepository repository, AppDbContext context, IWebHostEnvironment environment) : base(context)
        {
            _repository = repository;
            _environment = environment;
        }

        public async Task<List<Character>> GetAllCharactersAsync() =>
            await _repository.GetAllAsync();

        public async Task<Character?> GetCharacterByIdAsync(long id) =>
            await _repository.GetByIdAsync(id);

        public async Task<List<CharacterDto>> GetCharactersByUserIdAsync(Guid userId) =>
            (await _repository.GetByUserIdAsync(userId))
            .Select(c => new CharacterDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Imagem = c.Imagem,
                Name = c.Name,
                CharacterXID = c.CharacterXID,
                Gender = c.Gender,
                Height = c.Height,
                Weight = c.Weight,
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
                Marks = c.Marks
            }).ToList();

        public async Task<Character> CreateCharacterAsync(Guid userId, CreateCharacterDto characterDto, IFormFile? Imagem = null)
        {
            var imagemUrl = string.IsNullOrWhiteSpace(characterDto.Imagem) ? "/images/default.png" : characterDto.Imagem ?? string.Empty;

            if (Imagem != null)
            {
                imagemUrl = await SaveImageAndGetUrlAsync(Imagem);
            }

            // Aqui você pode usar o contexto para buscar o usuário, se necessário
            // var user = await _context.User.FindAsync(userId);

            var character = new Character
            {
                UserId = userId,
                Imagem = imagemUrl,
                Name = characterDto.Name,
                CharacterXID = characterDto.CharacterXID,
                Gender = characterDto.Gender,
                Height = characterDto.Height,
                Weight = characterDto.Weight,
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
            return await _repository.CreateAsync(character);
        }

        public async Task UpdateCharacterAsync(long id, UpdateCharacterDto updatedCharacter, IFormFile? Imagem = null)
        {
            var character = await _repository.GetByIdAsync(id);
            if (character == null)
            {
                throw new KeyNotFoundException("Character not found.");
            }

            if (Imagem != null)
            {
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
            character.Height = updatedCharacter.Height;
            character.Weight = updatedCharacter.Weight;
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

            await _repository.UpdateAsync(character);
        }

        // Antes da refatoração (dentro de CharacterService):
        // public async Task DeleteCharacterAsync(long id)
        // {
        //     var characterToDelete = await _context.Characters.FindAsync(id);
        //     if (characterToDelete != null)
        //     {
        //         _context.Characters.Remove(characterToDelete);
        //         await _context.SaveChangesAsync();
        //     }
        // }
        // Como o delete era genérico, agora usamos a classe base DeleteService:

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