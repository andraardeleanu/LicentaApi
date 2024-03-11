using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Core.Entities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class ProductController : ControllerBase
    { 
        private readonly IGenericService<Product> _productService;

        public ProductController(IGenericService<Product> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize]
        [Route("getProducts")]
        public async Task<IActionResult> GetProductAsync()
        {
            var products = await _productService.ListAsync();
            var dtoList = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Author));

            return new JsonResult(dtoList);
        }

        [HttpPost]
        [Authorize]
        [Route("addProduct")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductRequest productRequest)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
            var productEntity = productRequest.ToProductEntity(username);
            var workpoint = await _productService.AddAsync(productEntity);

            return new JsonResult(workpoint);
        }

        [HttpPut]
        //[Authorize]
        [Route("updateProduct")]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductDTO productRequest)
        {
            var product = await _productService.GetByIdAsync(productRequest.Id);

            product.Name = productRequest.Name;
            product.Price = productRequest.Price;
            product.DateUpdated = DateTime.UtcNow;

            await _productService.UpdateAsync(product);

            var entityResult = await _productService.GetByIdAsync(productRequest.Id);
            return new JsonResult(entityResult);
        }

        [HttpDelete]
        [Authorize]
        [Route("removeProduct")]
        public async Task<IActionResult> RemoveProductAsync(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            await _productService.DeleteAsync(product);

            return Ok();
        }
    }
}
