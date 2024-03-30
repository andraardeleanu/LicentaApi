using Core.Entities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api2.Controllers
{
    public class StockController : ControllerBase
    {
        private readonly IGenericService<Stock> _stockService;
        private readonly IGenericService<Product> _productService;

        public StockController(IGenericService<Stock> stockService, IGenericService<Product> productService)
        {
            _stockService = stockService;
            _productService = productService;
        }

        /*
        [HttpGet]
        //[Authorize]
        [Route("getStocks")]
        public async Task<IActionResult> GetStocksAsync()
        {
            var stocks = await _stockService.ListAsync();
            var resList = new List<StockDTO>();

            foreach (var stock in stocks)
            {
                var stockProduct = await _productService.WhereAsync(c => c.Id == stock.ProductId);

                foreach (var product in stockProduct)
                {
                    var stockEntity = new StockDTO(Id, ProductId, stockProduct.Name, AvailableStock, PendingStock);
                }               

            }
               

                return new JsonResult(dtoList);
            }
        }*/
    }
}
