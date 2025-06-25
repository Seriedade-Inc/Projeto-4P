using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pServer.Repository;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Services
{
    public class InventoryService : DeleteService<Inventory>
    {
        private readonly IInventoryRepository _repository;
        private readonly AppDbContext _context;

        public InventoryService(IInventoryRepository repository, AppDbContext context) : base(context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Inventory>> GetAllInventoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Inventory> CreateInventoryAsync(InventoryDto inventoryDto)
        {
            var character = await _context.Set<Character>().FindAsync(inventoryDto.CharacterId);
            if (character == null)
                throw new KeyNotFoundException("Character not found.");

            if (string.IsNullOrWhiteSpace(inventoryDto.ItemName))
                throw new ArgumentException("Invalid Item name.");

            var inventory = new Inventory
            {
                CharacterId = inventoryDto.CharacterId,
                ItemName = inventoryDto.ItemName,
                ItemDescription = inventoryDto.ItemDescription,
            };

            return await _repository.CreateAsync(inventory);
        }

        public async Task UpdateInventoryAsync(long id, InventoryDto inventoryDto)
        {
            var existingInventory = await _repository.GetByIdAsync(id);
            if (existingInventory == null)
                throw new KeyNotFoundException("Inventory item not found.");

            existingInventory.CharacterId = inventoryDto.CharacterId;
            existingInventory.ItemName = inventoryDto.ItemName;

            await _repository.UpdateAsync(existingInventory);
        }

        // Antes da refatoração:
        // public async Task DeleteInventoryAsync(long id)
        // {
        //     var inventoryToDelete = await _context.Set<Inventory>().FindAsync(id);
        //     if (inventoryToDelete != null)
        //     {
        //         _context.Set<Inventory>().Remove(inventoryToDelete);
        //         await _context.SaveChangesAsync();
        //     }
        // }
        // Como o delete era genérico, agora usamos a classe base DeleteService:
    }
}