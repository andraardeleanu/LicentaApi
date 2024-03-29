using Core.Entities;
using Core.Services.Interfaces;
using Api2.Requests;
using Api2.Mapping;
using Api2.Services.Interfaces;
using Api2.ApiModels;
using System.Linq;
using Core.Common;

namespace Api2.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderProduct> _orderProductService;
        private readonly IGenericService<Product> _productService;
        private readonly IGenericService<Stock> _productStockService;

        public OrderService(IGenericService<Order> orderService, IGenericService<Stock> productStockService, IGenericService<OrderProduct> orderProductService, IGenericService<Product> productService)
        {
            _orderService = orderService;
            _productStockService = productStockService;
            _orderProductService = orderProductService;
            _productService = productService;
        }

        private async Task RevertStock(IEnumerable<int> productIds, IEnumerable<ProductDetails> products)
        {
            foreach (var id in productIds)
            {
                var productDetails = products.SingleOrDefault(p => p.ProductId == id);
                if (productDetails != null)
                {
                    var stocks = await _productStockService.WhereAsync(x => x.ProductId == id);
                    if (stocks.Count > 0)
                    {
                        var stock = stocks.ElementAt(0);
                        stock.AvailableStock = stock.AvailableStock + productDetails.Quantity;
                        stock.PendingStock = stock.PendingStock - productDetails.Quantity;
                        await _productStockService.UpdateAsync(stock);
                    }
                }
            }
        }

        private async Task RevertOrderProduct(IEnumerable<int> orderProductIds)
        {
            foreach (var id in orderProductIds)
            {
                var orderProduct = await _orderProductService.GetByIdAsync(id);

                if (orderProduct != null)
                    await _orderProductService.DeleteAsync(orderProduct);
            }
        }

        private async Task<Stock?> CheckForStockStock(ProductDetails product)
        {
            var productResult = await _productService.GetByIdAsync(product.ProductId);

            if (productResult == null)
                return null;

            var stocks = await _productStockService.WhereAsync(x => x.ProductId == product.ProductId);

            if (stocks.Count == 0)
                return null;

            var stock = stocks.ElementAt(0);

            if (stock.AvailableStock < product.Quantity)
                return null;

            return stock;
        }


        private async Task<bool> UpdateStock(ProductDetails product, Stock stock)
        {
            try
            {
                stock.AvailableStock = stock.AvailableStock - product.Quantity;
                stock.PendingStock = stock.PendingStock + product.Quantity;

                await _productStockService.UpdateAsync(stock);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<GenericResponse<object>> AddOrderAsync(OrderRequest orderRequest, Enums.OrderType orderType)
        {
            var response = new GenericResponse<object>();
            try
            {
                var orderEntity = OrderMapper.ToOrderEntityCreate(orderRequest, orderRequest.Author);
                var order = await _orderService.AddAsync(orderEntity);
                if (order?.Id != null)
                {
                    List<int> orderProductIds = new List<int>();
                    List<int> productIds = new List<int>();
                    foreach (var product in orderRequest.Products)
                    {
                        var stock = await CheckForStockStock(product);
                        if (stock == null)
                            break;

                        var updateStockResponse = await UpdateStock(product, stock);
                        if (!updateStockResponse)
                            break;

                        productIds.Add(product.ProductId);

                        var orderProductEntity = new OrderProduct() { ProductId = product.ProductId, Quantity = product.Quantity, OrderId = order.Id };
                        var orderProductResult = await _orderProductService.AddAsync(orderProductEntity);
                        if (orderProductResult == null)
                            break;

                        orderProductIds.Add(orderProductResult.Id);
                    }
                    if (orderProductIds.Count == orderRequest.Products.Count && orderRequest.Products.Count == orderProductIds.Count)
                    {
                        response.StatusCode = 200;
                        response.Data = new { OrderId = order.Id };
                        return response;
                    }
                    else
                    {
                        await RevertStock(productIds, orderRequest.Products);
                        await RevertOrderProduct(orderProductIds);
                        await _orderService.DeleteAsync(order);
                        response.StatusCode = 500;
                        response.Data = new { ErrorMessage = "let's see" };
                        return response;
                    }
                }
                response.StatusCode = 500;
                response.Data = new { ErrorMessage = "Unable to insert order" };
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Data = new { ErrorMessage = ex };
                return response;
            }
        }
    }
}

