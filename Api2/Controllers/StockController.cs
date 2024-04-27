using Api2.ApiModels;
using Api2.Requests;
using Core.Constants;
using Core.Entities;
using Core.Models;
using Core.Services.Interfaces;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetStocksAsync([FromQuery] NameFilterRequest productFilterRequest)
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

            if (productFilterRequest.Name != null)
            {

                resList = resList.FindAll(x => x.ProductName.Contains(productFilterRequest.Name));
            }

            return new JsonResult(resList);
        }


        [HttpGet]
        //[Authorize]
        [Route("getStockById/{id}")]
        public async Task<IActionResult> GetStockByIdAsync(int id)
        {
            var stock = await _stockService.GetByIdAsync(id);

            return new JsonResult(stock);
        }

        [HttpGet]
        //[Authorize]
        [Route("getStockByProductId/{productId}")]
        public async Task<IActionResult> GetStockByProductIdAsync(int productId)
        {
            var productStock = await _stockService.WhereAsync(x => x.ProductId == productId);

            return new JsonResult(productStock);
        }


        [HttpPost]
        //[Authorize]
        [Route("updateStock/")]
        public async Task<IActionResult> UpdateWorkpointAsync([FromBody] UpdateStockRequest updateStockRequest)
        {
            if (updateStockRequest.AvailableStock <= 0)
            {
                return BadRequest(new Result(ErrorMessages.InvalidStock));
            }

            try
            {
                var stock = await _stockService.GetByIdAsync(updateStockRequest.StockId);

                stock.AvailableStock = updateStockRequest.AvailableStock;
                stock.DateUpdated = DateTime.UtcNow;

                await _stockService.UpdateAsync(stock);

                var entityResult = await _stockService.GetByIdAsync(updateStockRequest.StockId);
                return new JsonResult(entityResult);

            }
            catch (Exception ex)
            {
                return BadRequest(new Result(ErrorMessages.InvalidData));
            }
        }
    }

}
