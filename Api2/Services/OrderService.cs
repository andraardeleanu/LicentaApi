using Core.Entities;
using Core.Services.Interfaces;
using Api2.Requests;
using Api2.Mapping;
using Api2.Services.Interfaces;
using Api2.ApiModels;
using System.Linq;

namespace Api2.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderProduct> _orderProductService;
        private readonly IGenericService<Product> _productService;
        private readonly IGenericService<Stock> _productStockService;

        public OrderService(IGenericService<Order> orderService, IGenericService<OrderProduct> orderProductService, IGenericService<Product> productService)
        {
            _orderService = orderService;
            _orderProductService = orderProductService;
            _productService = productService;
        }

        private async Task<List<int>> RevertStock (IEnumerable<int> productIds, IEnumerable<ProductDetails> products)
        {
            List<int> stocksWithErrors = new List<int>();
            foreach( var id in productIds)
            {
                var productDetails = products.SingleOrDefault(p => p.ProductId == id);
                if (productDetails != null)
                {
                    var stock = await _productStockService.WhereAsync(x => x.ProductId == id);
                    stock.AvailableStock = stock.AvailableStock + productDetails.Quantity;
                    stock.PendingStock = stock.PendingStock - productDetails.Quantity;
                    var newStock = await _productStockService.UpdateAsync(stock);
                    if (newStock == null)
                        stocksWithErrors.Add(id);
                }
            }
        }

        private async Task RevertOrderProduct(IEnumerable<int> orderProductIds)
        {
            foreach (var id in orderProductIds)
            {
                    var orderProduct = await _orderProductService.GetByIdAsync(id);

                if (orderProduct != null)
                    var deleteResult = _orderProductService.DeleteAsync(orderProduct);
            }
            
        }

        private async Task<Stock> CheckForStockStock(ProductDetails product)
        {
            var productResult = await _productService.GetByIdAsync(product.ProductId);

            if (productResult == null)
                null;
            var stock = await _productStockService.WhereAsync(x => x.ProductId == product.ProductId);
            if (stock == null)
                null;

            if (stock.AvailableStock < product.Quantity)
                null;

            return stock;
        }


        private async Task <bool> UpdateStock(ProductDetails product, Stock stock)
        { 
            stock.AvailableStock = stock.AvailableStock - product.Quantity;
            tock.PendingStock = stock.PendingStock + product.Quantity;

            var newStock = await _productStockService.UpdateAsync(stock);
            if (newStock == null)
                return false;


            return true;
        }

        public async Task<GenericResponse<object>> AddOrderAsync(OrderRequest orderRequest)
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
                        if (stock==null)
                            break;

                        var updateStockResponse = await UpdateStock(product);
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
                        response.Status = 200;
                        response.Data = order;
                        return response;
                    }
                    else
                    {
                        var stockWithErrors = await RevertStock(productIds, orderRequest.Products);
                        var removeOrders = await RevertOrderProduct(orderProductIds);
                        response.Status = 500;
                        response.Data = new { ErrorMessage = "let's see" };
                    }
                }
                response.Status = 500;
                response.Data = new { ErrorMessage = "Unable to insert order" };
                
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Data = new { ErrorMessage = ex };
            }
        }

    }
}

