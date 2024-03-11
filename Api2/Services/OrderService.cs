using System;
using Core.Entities;
using Core.Services.Interfaces;
using Api2.Requests;
using Api2.Mapping;
using System.Security.Claims;

namespace Core.Services
{
	public class OrderService: IOrderService
	{
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderProduct> _orderProductService;
        private readonly IGenericService<Product> _productService;

        public OrderService(IGenericService<Order> orderService, IGenericService<OrderProduct> orderProductService, IGenericService<Product> productService )
        {
            _orderService = orderService;
            _orderProductService = orderProductService;
            _productService = productService;
        }

        public async Task<bool> AddOrderAsync(OrderRequest orderRequest,string username)
        {
            try
            {
                var orderEntity = OrderMapper.ToOrderEntityCreate(orderRequest, username);
                var order = await _orderService.AddAsync(orderEntity);
                if (order?.Id != null)
                {
                    foreach (var product in orderRequest.Products)
                    {
                        //check for quantity
                        var orderProductEntity = new OrderProduct() { ProductId = product.ProductId, Quantity = product.Quantity, OrderId=order.Id };
                        var result = await _orderProductService.AddAsync(orderProductEntity);
                    }
                }
                return true;
            } catch(Exception ex) { }
            return false;
        }

    }
}

