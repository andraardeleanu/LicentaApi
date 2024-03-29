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
        private readonly IGenericService<Stock> _stockService;

        public ProductController(IGenericService<Product> productService, IGenericService<Stock> stockService)
        {
            _productService = productService;
            _stockService = stockService;
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
        //[Authorize]
        [Route("addProduct")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductRequest productRequest, int availableStock)
        {
            var existingProduct = await _productService.WhereAsync(x => x.Name == productRequest.Name);
            if (existingProduct != null && existingProduct.Any()) return BadRequest("Exista deja un produs cu acest nume.");
            if (availableStock <= 0) return BadRequest("Nu se pot crea produse cu stoc mai mic sau egal cu 0.");

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
            var productEntity = productRequest.ToProductEntity(username);
            var product = await _productService.AddAsync(productEntity);

            var stockEntity = new Stock
            {
                ProductId = product.Id,
                AvailableStock = availableStock,
                PendingStock = 0,
                Author = username,
                DateCreated = DateTime.UtcNow              
            };

            var stock = await _stockService.AddAsync(stockEntity);

            return new JsonResult(product);
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

        [HttpGet]
        //[Authorize]
        [Route("getProductsByName/{name}")]
        public async Task<IActionResult> GetProductsByNameAsync(string name)
        {
            var product = await _productService.WhereAsync(x => x.Name.Contains(name));

            return new JsonResult(product);
        }
    }
}
