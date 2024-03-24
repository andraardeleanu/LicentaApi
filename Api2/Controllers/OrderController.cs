﻿using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Api2.Services.Interfaces;
using Core.Entities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;


namespace Api2.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderProduct> _orderProductService;
        private readonly IOrderService _orderServiceTest;

        public OrderController(IGenericService<Order> orderService, IGenericService<OrderProduct> orderProductService, IOrderService orderServiceTest)
        {
            _orderService = orderService;
            _orderProductService = orderProductService;
            _orderServiceTest = orderServiceTest;
        }

        [HttpGet]
        [Authorize]
        [Route("getOrders")]
        public async Task<IActionResult> GetOrderAsync()
        {
            var orders = await _orderService.ListAsync();
            var dtoList = orders.Select(x => new OrderDTO(x.Id, x.OrderNo, x.Date, x.WorkPointId, x.Status));

            return new JsonResult(dtoList);
        }

        [HttpGet]
        [Route("getOrderDetails/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsAsync(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId, x => x.OrderProduct);
            var orderProducts = await _orderProductService.WhereAsync(x => x.OrderId == orderId, y=> y.Product);

            var productsWithQuantity = orderProducts.Select(op => new ProductWithQuantity
            {
                Name = op.Product.Name,
                Price = op.Product.Price,
                Quantity = op.Quantity
            }).ToList();

            var orderDetail = OrderMapper.ToOrderDetailsDTO(order, productsWithQuantity);

            return new JsonResult(orderDetail.Products.ToList());
        }

        [HttpPost]
       // [Authorize]
        [Route("addOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest orderRequest)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
            orderRequest.Author = username;
            var res = await _orderServiceTest.AddOrderAsync(orderRequest);
            return new JsonResult(res);
        }
    }
}
