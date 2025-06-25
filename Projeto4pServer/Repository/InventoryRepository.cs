using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pSharedLibrary.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projeto4pServer.Repository
{
    public interface IInventoryRepository
    {
        Task<List<Inventory>> GetAllAsync();
        Task<Inventory?> GetByIdAsync(long id);
        Task<Inventory> CreateAsync(Inventory inventory);
        Task UpdateAsync(Inventory inventory);
    }

    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetAllAsync()
        {
            return await _context.Set<Inventory>()
                .Include(i => i.Character)
                .ToListAsync();
        }

        public async Task<Inventory?> GetByIdAsync(long id)
        {
            return await _context.Set<Inventory>()
                .Include(i => i.Character)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory> CreateAsync(Inventory inventory)
        {
            _context.Set<Inventory>().Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task UpdateAsync(Inventory inventory)
        {
            _context.Entry(inventory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}