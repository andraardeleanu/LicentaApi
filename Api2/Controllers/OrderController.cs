using Api2.ApiModels;
using Api2.Requests;
using Core.Entities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> _orderService;

        public OrderController(IGenericService<Order> orderService)
        {
            _orderService = orderService;
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
    }
}
