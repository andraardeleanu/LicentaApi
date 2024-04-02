using Api2.ApiModels;
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


        [HttpGet]
        //[Authorize]
        [Route("getStocks")]
        public async Task<IActionResult> GetStocksAsync()
        {
            var stocks = await _stockService.ListAsync();
            var resList = new List<StockDTO>();

            foreach (var stock in stocks)
            {
                var stockProducts = await _productService.WhereAsync(p => p.Id == stock.ProductId);

                foreach (var sp in stockProducts)
                {
                    var stockEntity = new StockDTO(stock.Id, stock.ProductId, sp.Name, stock.AvailableStock, stock.PendingStock);
                    resList.Add(stockEntity);
                }                              
            }
            return new JsonResult(resList);
        }


        [HttpPut]
        //[Authorize]
        [Route("updateStock")]
        public async Task<IActionResult> UpdateWorkpointAsync([FromBody] StockDTO stockRequest)
        {
            var stock = await _stockService.GetByIdAsync(stockRequest.Id);

            stock.AvailableStock = stockRequest.AvailableStock;
            stock.DateUpdated = DateTime.UtcNow;

            await _stockService.UpdateAsync(stock);

            var entityResult = await _stockService.GetByIdAsync(stockRequest.Id);
            return new JsonResult(entityResult);
        }
    }

}
