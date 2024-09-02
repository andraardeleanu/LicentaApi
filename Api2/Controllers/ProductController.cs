using Core.ApiModels;
using Core.Constants;
using Core.Entities;
using Core.Mapping;
using Core.Models;
using Core.Requests;
using Core.Responses;
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("addProduct")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductRequest productRequest)
        {
            if (string.IsNullOrWhiteSpace(productRequest.Name))
            {
                return BadRequest(new Result(ErrorMessages.AllFieldsAreMandatory));
            }
            else if (productRequest.AvailableStock <= 0)
            {
                return BadRequest(new Result(ErrorMessages.InvalidStock));
            }
            else if (productRequest.Price <= 0)
            {
                return BadRequest(new Result(ErrorMessages.InvalidPrice));
            }
            else
            {
                var existingProduct = await _productService.WhereAsync(x => x.Name == productRequest.Name);
                if (existingProduct != null && existingProduct.Any()) return BadRequest(new Result(ErrorMessages.ExistingProduct));
            }

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
            var productEntity = productRequest.ToProductEntity(username);
            var product = await _productService.AddAsync(productEntity);

            var stockEntity = new Stock
            {
                ProductId = product.Id,
                AvailableStock = productRequest.AvailableStock,
                PendingStock = 0,
                Author = username,
                DateCreated = DateTime.UtcNow
            };

            var stock = await _stockService.AddAsync(stockEntity);
            var productResponse = new ProductResponse { ProductId = product.Id, ProductName = product.Name, StockId = stock.Id, AvailableStock = stockEntity.AvailableStock };

            return Ok(new Result<ProductResponse>(productResponse));
        }

        [HttpGet]
        [Authorize]
        [Route("getProducts")]
        public async Task<IActionResult> GetProductAsync([FromQuery] NameFilterRequest productFilterRequest)
        {
            var products = await _productService.ListAsync();

            if (productFilterRequest.Name != null)
            {
                products = products.FindAll(x => x.Name.Contains(productFilterRequest.Name));
            }

            var dtoList = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price));
            return new JsonResult(dtoList);
        }        

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("updateProduct")]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductDTO productRequest)
        {
            try
            {
                var product = await _productService.GetByIdAsync(productRequest.Id);

                var existingProduct = await _productService.WhereAsync(x => x.Name == productRequest.Name);
                if (existingProduct != null && existingProduct.Any()) return BadRequest(new Result(ErrorMessages.ExistingProduct));

                if (productRequest.Price <= 0)
                {
                    return BadRequest(new Result(ErrorMessages.InvalidPrice));
                }

                product.Name = productRequest.Name;
                product.Price = productRequest.Price;
                product.DateUpdated = DateTime.UtcNow;

                await _productService.UpdateAsync(product);

                var entityResult = await _productService.GetByIdAsync(productRequest.Id);
                return new JsonResult(entityResult);

            }
            catch (Exception)
            {
                return BadRequest(new Result(ErrorMessages.ProductNotFound));
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("removeProduct")]
        public async Task<IActionResult> RemoveProductAsync(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            var stock = await _stockService.WhereAsync(x => x.ProductId == id);
            await _productService.DeleteAsync(product);
            await _stockService.DeleteAsync(stock.FirstOrDefault()!);

            var productResponse = new ProductResponse { ProductId = product.Id };
            return Ok(new Result<ProductResponse>(productResponse));
        }
    }
}
