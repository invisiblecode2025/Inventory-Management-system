using Inventory.ManagementDto;
using Inventory.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ItemsController : Controller
    {

        IItemServices _itemsServices;
        public ItemsController(IItemServices itemsServices)
        {
            _itemsServices = itemsServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _itemsServices.GetAllAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemDto item)
        {
            return Ok(await _itemsServices.Create(item));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ItemDto item)
        {
            return Ok(await _itemsServices.Update(item));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ItemDto item)
        {
            return Ok(await _itemsServices.Update(item));
        }
    }
}
