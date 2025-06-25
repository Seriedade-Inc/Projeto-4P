using Projeto4pSharedLibrary.Classes;
using Projeto4pServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projeto4pServer.Repository
{
    public interface IBlasphemyRepository
    {
        Task<List<Blasphemy>> GetAllAsync();
        Task<Blasphemy?> GetByIdAsync(long id);
        Task<Blasphemy> CreateAsync(Blasphemy blasphemy);
        Task UpdateAsync(Blasphemy blasphemy);
    }

    public class BlasphemyRepository : IBlasphemyRepository
    {
        private readonly AppDbContext _context;

        public BlasphemyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Blasphemy>> GetAllAsync()
        {
            return await _context.Set<Blasphemy>().ToListAsync();
        }

        public async Task<Blasphemy?> GetByIdAsync(long id)
        {
            return await _context.Set<Blasphemy>().FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Blasphemy> CreateAsync(Blasphemy blasphemy)
        {
            _context.Set<Blasphemy>().Add(blasphemy);
            await _context.SaveChangesAsync();
            return blasphemy;
        }

        public async Task UpdateAsync(Blasphemy blasphemy)
        {
            _context.Entry(blasphemy).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}