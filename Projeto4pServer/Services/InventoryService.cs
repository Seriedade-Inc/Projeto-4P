using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Services
{
    public class InventoryService : DeleteService<Inventory>
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetAllInventoriesAsync()
        {
            return await _context.Set<Inventory>()
                .Include(i => i.Character)
                .ToListAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(long id)
        {
            return await _context.Set<Inventory>()
                .Include(i => i.Character)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory> CreateInventoryAsync(InventoryDto inventoryDto)
        {
            var character = await _context.Set<Character>().FindAsync(inventoryDto.CharacterId);
            if (character == null)
            {
                throw new KeyNotFoundException("Character not found.");
            }

            if (string.IsNullOrWhiteSpace(inventoryDto.ItemName) || inventoryDto.Quantity <= 0)
            {
                throw new ArgumentException("Invalid item name or quantity.");
            }

            var inventory = new Inventory
            {
                CharacterId = inventoryDto.CharacterId,
                ItemName = inventoryDto.ItemName,
                ItemDescription = inventoryDto.ItemDescription,
                Quantity = inventoryDto.Quantity
            };

            _context.Set<Inventory>().Add(inventory);
            await _context.SaveChangesAsync();

            return inventory;
        }

        public async Task UpdateInventoryAsync(long id, InventoryDto inventoryDto)
        {
            var existingInventory = await _context.Set<Inventory>().FindAsync(id);
            if (existingInventory == null)
            {
                throw new KeyNotFoundException("Inventory item not found.");
            }

            existingInventory.CharacterId = inventoryDto.CharacterId;
            existingInventory.ItemName = inventoryDto.ItemName;
            existingInventory.Quantity = inventoryDto.Quantity;

            _context.Entry(existingInventory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}