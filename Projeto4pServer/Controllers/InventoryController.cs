using Microsoft.AspNetCore.Mvc;
using Projeto4pServer.DTOs;
using Projeto4pServer.Services;

namespace Projeto4pServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _service;

        public InventoryController(InventoryService service)
        {
            _service = service;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<IActionResult> GetInventories()
        {
            var inventories = await _service.GetAllInventoriesAsync();
            return Ok(inventories);
        }

        // GET: api/Inventory/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventory(long id)
        {
            var inventory = await _service.GetInventoryByIdAsync(id);
            if (inventory == null)
                return NotFound("Inventory item not found.");

            return Ok(inventory);
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryDto inventoryDto)
        {
            try
            {
                var inventory = await _service.CreateInventoryAsync(inventoryDto);
                return CreatedAtAction(nameof(GetInventory), new { id = inventory.Id }, inventory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Inventory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(long id, [FromBody] InventoryDto inventoryDto)
        {
            try
            {
                await _service.UpdateInventoryAsync(id, inventoryDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Inventory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(long id)
        {
            try
            {
                await _service.DeleteAsync(id); // Reutiliza o método genérico de delete do DeleteService
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}