using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOwnerCore.Application_Core.Entities.View_Model;
using ShopOwnerCore.Application_Core.Helper.Pager;
using ShopOwnerCore.Application_Core.Interface;

namespace ShopOwnerCore.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(Guid ShopId, [FromQuery]ProductParameter parameter)
        {
            var Products = await _productService.GetAllProduct(ShopId,parameter,true);
            if(Products== null)
            {
                _logger.LogInformation("Products does not exist in the database");
                return NotFound();
            }
            return Ok(Products);
        }

        [HttpGet("{id}", Name = "GetProductForShop")]
        public async Task<IActionResult> GetProductById(Guid Id)
        {
            var Product = await _productService.GetProductById(Id);
            return Ok(Product);
        }

        

        [HttpPost("{ShopId}/Product")]
        public async Task<IActionResult> CreateProductForShop([FromBody]ProductViewModel model, Guid ShopId)
        {
            if(model == null)
            {
                _logger.LogError("Product object sent from the client is null");
                return BadRequest();
            }
            await _productService.CreateProductForShop(model,ShopId);
            return CreatedAtRoute("GetProductForShop", new { ShopId, Id = model.Id },null );
        }

        [HttpGet("{keyword}")]
        public async Task<IActionResult> SearchProduct(string keyword)
        {
           var Product= await _productService.GetProduct(keyword);
            if (Product == null)
            {
                _logger.LogInformation($"Product:{keyword}");
                return NotFound();
            }
            

            
            return Ok(keyword);
        }

        [HttpPut("{ShopId}")]

        public async Task<IActionResult> UpdateProductInShop([FromBody]ProductViewModel model, Guid ShopId, Guid ProductId)
        {
            await _productService.UpdateProductInShop(model, ShopId,ProductId);
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }


      
            
    }
}
