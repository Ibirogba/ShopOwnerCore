using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOwnerCore.Application_Core.Entities.View_Model;
using ShopOwnerCore.Application_Core.Interface;

namespace ShopOwnerCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly ILogger<ShopController> _logger;

        public ShopController(IShopService shopService, ILogger<ShopController> logger)
        {
            _shopService = shopService;
            _logger = logger;
        }

        [HttpGet(Name = "GetAllShop")]
        public async Task<IActionResult> GetAllShop()
        {
            var Shop = await _shopService.GetAllShops();
            if (Shop == null)
            {
                _logger.LogError("");
                return NotFound();
            }
            return Ok(Shop);
        }
        [HttpPost]
        public async Task<IActionResult> CreateShop([FromBody] ShopViewModel model)
        {
            if (model == null)
            {
                _logger.LogError("Shop object from client is null");
                return BadRequest();

            }
            await _shopService.CreateShop(model);
            return CreatedAtRoute("ShopId", new { id = model.Id },null);



        }

        [HttpGet("{Id}", Name = "ShopId")]

        public async Task<IActionResult> GetById(Guid Id)
        {
            var Shop = await _shopService.GetShopById(Id);
            return Ok(Shop);

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteShop(Guid Id)
        {
            await _shopService.DeleteShop(Id);
            return NoContent();
        }


        

        
    }
}
