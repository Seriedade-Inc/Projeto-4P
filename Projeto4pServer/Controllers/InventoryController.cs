using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
using Projeto4pServer.DTOs;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<IActionResult> GetInventories()
        {
            var inventories = await _context.Set<Inventory>()
                .Include(i => i.Character)
                .ToListAsync();

            // Mapeia para DTO
            // var inventoryDtos = inventories.Select(i => new InventoryDto
            // {
            //     ItemName = i.ItemName,
            //     Quantity = i.Quantity
            // }).ToList();

            return Ok(inventories);
        }

        // GET: api/Inventory/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventory(long id)
        {
            var inventory = await _context.Set<Inventory>()
                .Include(i => i.Character)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
                return NotFound("Inventory item not found.");

            // Mapeia para DTO
            // var inventoryDto = new InventoryDto
            // {
            //     ItemName = inventory.ItemName,
            //     Quantity = inventory.Quantity
            // };

            return Ok(inventory);
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryDto inventoryDto)
        {
            // Valida se o personagem existe
            var character = await _context.Set<Character>().FindAsync(inventoryDto.CharacterId);
            if (character == null)
                return NotFound("Character not found.");

            if (string.IsNullOrWhiteSpace(inventoryDto.ItemName) || inventoryDto.Quantity <= 0)
                return BadRequest("Invalid item name or quantity.");

             if (inventoryDto.CharacterId == null || 
                !await _context.Agendas.AnyAsync(a => a.Id == inventoryDto.CharacterId))
            {
                return BadRequest("Invalid CharacterId. The chracter does not exist.");
            }

            // Mapeia o DTO para a entidade
            var inventory = new Inventory
            {
                CharacterId = inventoryDto.CharacterId,
                ItemName = inventoryDto.ItemName,
                Quantity = inventoryDto.Quantity
            };

            _context.Set<Inventory>().Add(inventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventory), new { id = inventory.Id }, inventoryDto);
        }

        // PUT: api/Inventory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(long id, [FromBody] InventoryDto inventoryDto)
        {
            var existingInventory = await _context.Set<Inventory>().FindAsync(id);
            if (existingInventory == null)
                return NotFound("Inventory item not found.");

            // Atualiza os valores da entidade com base no DTO
            existingInventory.CharacterId = inventoryDto.CharacterId;
            existingInventory.ItemName = inventoryDto.ItemName;
            existingInventory.Quantity = inventoryDto.Quantity;

            _context.Entry(existingInventory).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Inventory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(long id)
        {
            var inventory = await _context.Set<Inventory>().FindAsync(id);
            if (inventory == null)
                return NotFound("Inventory item not found.");

            _context.Set<Inventory>().Remove(inventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}