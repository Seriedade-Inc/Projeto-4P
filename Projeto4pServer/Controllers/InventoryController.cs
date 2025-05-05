using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto4pServer.Data;
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

            return Ok(inventory);
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] Inventory inventory)
        {
            var character = await _context.Set<Character>().FindAsync(inventory.CharacterId);
            if (character == null)
                return NotFound("Character not found.");

            _context.Set<Inventory>().Add(inventory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInventory), new { id = inventory.Id }, inventory);
        }

        // PUT: api/Inventory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(long id, [FromBody] Inventory inventory)
        {
            if (id != inventory.Id)
                return BadRequest("Inventory ID mismatch.");

            var existingInventory = await _context.Set<Inventory>().FindAsync(id);
            if (existingInventory == null)
                return NotFound("Inventory item not found.");

            existingInventory.ItemName = inventory.ItemName;
            existingInventory.Quantity = inventory.Quantity;

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