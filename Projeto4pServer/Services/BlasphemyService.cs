using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Services
{
    public class BlasphemyService : DeleteService<Blasphemy>
    {
        private readonly AppDbContext _context;

        public BlasphemyService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Blasphemy>> GetAllBlasphemiesAsync()
        {
            return await _context.Set<Blasphemy>()
                .ToListAsync();
        }

        public async Task<Blasphemy?> GetBlasphemyByIdAsync(long id)
        {
            return await _context.Set<Blasphemy>()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Blasphemy> CreateBlasphemyAsync(BlasphemyDto blasphemyDto)
        {
            var blasphemy = new Blasphemy
            {
                CharacterId = blasphemyDto.CharacterId,
                BlasphemyName = blasphemyDto.BlasphemyName,
                BlasphemyText = blasphemyDto.BlasphemyText
            };

            _context.Set<Blasphemy>().Add(blasphemy);
            await _context.SaveChangesAsync();

            return blasphemy;
        }

        public async Task UpdateBlasphemyAsync(long id, BlasphemyDto blasphemyDto)
        {
            var existingBlasphemy = await _context.Set<Blasphemy>()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existingBlasphemy == null)
            {
                throw new KeyNotFoundException("Blasphemy not found.");
            }

            existingBlasphemy.CharacterId = blasphemyDto.CharacterId;
            existingBlasphemy.BlasphemyName = blasphemyDto.BlasphemyName;
            existingBlasphemy.BlasphemyText = blasphemyDto.BlasphemyText;

            _context.Entry(existingBlasphemy).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}