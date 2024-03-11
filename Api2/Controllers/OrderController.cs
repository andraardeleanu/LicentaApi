using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
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

        public OrderController(IGenericService<Order> orderService, IGenericService<OrderProduct> orderProductService)
        {
            _orderService = orderService;
            _orderProductService = orderProductService;
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
        [Route("getOrderDetails")]
        public async Task<IActionResult> GetOrderDetailsAsync(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId, x => x.OrderProduct);
            var orderProducts = await _orderProductService.WhereAsync(x => x.OrderId == orderId, y=> y.Product);

            var orderDetail = OrderMapper.ToOrderDetailsDTO(order, orderProducts.Select(x => x.Product).ToList());

            return Ok(JsonConvert.SerializeObject(orderDetail));
        }

        [HttpPost]
        [Authorize]
        [Route("addOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest orderRequest)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
            var orderEntity = OrderMapper.ToOrderEntityCreate(orderRequest,username);
            var order = await _orderService.AddAsync(orderEntity);
            return new JsonResult(order);
        }
    }
}
