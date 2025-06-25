using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;
using Projeto4pServer.Repository;

namespace Projeto4pServer.Services
{
    public class BlasphemyService : DeleteService<Blasphemy>
    {
        private readonly IBlasphemyRepository _repository;

        public BlasphemyService(IBlasphemyRepository repository, AppDbContext context) : base(context)
        {
            _repository = repository;
        }

        public async Task<List<Blasphemy>> GetAllBlasphemiesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Blasphemy?> GetBlasphemyByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Blasphemy> CreateBlasphemyAsync(BlasphemyDto blasphemyDto)
        {
            var blasphemy = new Blasphemy
            {
                CharacterId = blasphemyDto.CharacterId,
                BlasphemyName = blasphemyDto.BlasphemyName,
                BlasphemyText = blasphemyDto.BlasphemyText
            };

            return await _repository.CreateAsync(blasphemy);
        }

        public async Task UpdateBlasphemyAsync(long id, BlasphemyDto blasphemyDto)
        {
            var existingBlasphemy = await _repository.GetByIdAsync(id);

            if (existingBlasphemy == null)
            {
                throw new KeyNotFoundException("Blasphemy not found.");
            }

            existingBlasphemy.CharacterId = blasphemyDto.CharacterId;
            existingBlasphemy.BlasphemyName = blasphemyDto.BlasphemyName;
            existingBlasphemy.BlasphemyText = blasphemyDto.BlasphemyText;

            await _repository.UpdateAsync(existingBlasphemy);
        }

        // Antes da refatoração (dentro de BlasphemyService):
        // public async Task DeleteBlasphemyAsync(long id)
        // {
        //     var blasphemyToDelete = await _context.Set<Blasphemy>().FindAsync(id);
        //     if (blasphemyToDelete != null)
        //     {
        //         _context.Set<Blasphemy>().Remove(blasphemyToDelete);
        //         await _context.SaveChangesAsync();
        //     }
        // }
        // Como o delete era genérico, agora usamos a classe base DeleteService:
    }
}