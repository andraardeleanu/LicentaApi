using Core.Entities;
using Core.Services.Interfaces;
using Api2.Requests;
using Api2.Mapping;
using Api2.Services.Interfaces;
using Api2.ApiModels;
using System.Linq;
using Core.Common;
using DocumentFormat.OpenXml.Drawing;
using System.Runtime.CompilerServices;
using Core.Constants;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

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

        private async Task<decimal> GetProductPrice(int productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            if (product != null)
            {
                return product.Price;
            }
            else
            {
                throw new Exception(ErrorMessages.ProductNotFound);
            }
        }

        private async Task<bool> GetProductById(int productId)
        {
            var product = await _productService.WhereAsync(x => x.Id == productId);
            if (product != null && product.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<GenericResponse<object>> AddOrderAsync(OrderRequest orderRequest, Enums.OrderType orderType)
        {
            var response = new GenericResponse<object>();
            var errors = new List<string>();
            try
            {
                var orderEntity = OrderMapper.ToOrderEntityCreate(orderRequest, orderRequest.Author);
                var order = await _orderService.AddAsync(orderEntity);
                if (order?.Id != null)
                {
                    List<int> orderProductIds = new List<int>();
                    List<int> productIds = new List<int>();
                    decimal totalPrice = 0;
                    foreach (var product in orderRequest.Products)
                    {
                        if (await GetProductById(product.ProductId) == false)
                        {
                            errors.Add(ErrorMessages.ProductNotFound);
                            continue;
                        }
                        var stock = await CheckForStockStock(product);
                        if (stock == null)
                        {
                            errors.Add(ErrorMessages.AvailableStockError);
                            continue;
                        }
                        var updateStockResponse = await UpdateStock(product, stock);
                        if (!updateStockResponse)
                        {
                            errors.Add(ErrorMessages.InvalidData);
                            continue;
                        }
                        productIds.Add(product.ProductId);
                        var orderProductEntity = new OrderProduct()
                        {
                            ProductId = product.ProductId,
                            Quantity = product.Quantity,
                            OrderId = order.Id
                        };
                        var orderProductResult = await _orderProductService.AddAsync(orderProductEntity);
                        if (orderProductResult == null)
                        {
                            errors.Add(ErrorMessages.InvalidData);
                            continue;
                        }
                        orderProductIds.Add(orderProductResult.Id);
                        var productPrice = await GetProductPrice(product.ProductId);
                        totalPrice += productPrice * product.Quantity;
                    }
                    if (orderProductIds.Count == orderRequest.Products.Count && orderRequest.Products.Count == orderProductIds.Count)
                    {
                        response.StatusCode = 200;
                        response.Data = new
                        {
                            OrderId = order.Id,
                            TotalPrice = totalPrice
                        };
                        return response;
                    }
                    else
                    {
                        await RevertStock(productIds, orderRequest.Products);
                        await RevertOrderProduct(orderProductIds);
                        await _orderService.DeleteAsync(order);
                        errors.Add(ErrorMessages.DuplicatedProduct);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            finally
            {
                if (errors.Any())
                {
                    response.StatusCode = 500;
                    response.Data = errors.FirstOrDefault();
                }
            }
            return response;
        }
    }
}
