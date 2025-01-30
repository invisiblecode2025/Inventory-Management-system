using Inventory.ManagementDto;
using Inventory.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InventoryController : Controller
    {
        IInventoryServices _inventoryServices;
        public InventoryController(IInventoryServices inventoryServices)
        {
            _inventoryServices = inventoryServices;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(InventoryFilterDto inventoryFilterDto)
        {
            return Ok(await _inventoryServices.GetAllAsync(inventoryFilterDto));
        }
        [HttpPost]
        public async Task<IActionResult> Create(InventoryDto _inventoryDto)
        {
            return Ok(await _inventoryServices.Create(_inventoryDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(InventoryDto _inventoryDto)
        {
            return Ok(await _inventoryServices.Update(_inventoryDto));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(InventoryDto _inventoryDto)
        {
            return Ok(await _inventoryServices.Update(_inventoryDto));
        }
    }
}
