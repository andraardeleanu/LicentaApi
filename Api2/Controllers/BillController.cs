using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Api2.Responses;
using Api2.Services.Interfaces;
using AutoMapper;
using Core.Common;
using Core.Constants;
using Core.Entities;
using Core.Models;
using Core.Services.Interfaces;
using Infra.Data.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class BillController : ControllerBase
    {
        private readonly IGenericService<Bill> _billService;
        private readonly IGenericService<Order> _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillController(IGenericService<Order> orderService, IGenericService<Bill> billService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _billService = billService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("billGenerator")]
        public async Task<IActionResult> GenerateOrderBill([FromBody] BillRequest request)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            
            if (request.Status == Enums.OrderStatus.Initialized.ToString())
            {
                return BadRequest(new Result(ErrorMessages.OrderStatusBillError));
            }

            var existingBill = await _billService.WhereAsync(x => x.OrderNo == request.OrderNo);
            if (existingBill != null && existingBill.Any())
            {
                return BadRequest(new Result(ErrorMessages.BillInsertionError));
            }
            
            if (request != null)
            {
                var billEntity = request.ToBillEntity(user.Id);
                await _billService.AddAsync(billEntity);
               
                var billedOrder = (await _orderService.WhereAsync(x => x.OrderNo.ToString() == request.OrderNo)).FirstOrDefault();

                billedOrder.Status = Enums.OrderStatus.Billed.ToString();
                billedOrder.DateUpdated = DateTime.UtcNow;

                await _orderService.UpdateAsync(billedOrder);

                return Ok(new Result());
            }
            else
            {
                return BadRequest(new Result(ErrorMessages.InvalidData));
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("getBills")]
        public async Task<IActionResult> GetBillsAsync()
        {
            var bills = await _billService.ListAsync();

            bills = bills.OrderByDescending(x => x.DateCreated).ToList();

            var dtoList = bills.Select(x => new BillsDTO(x.OrderNo, x.DateCreated, x.CreatedBy, x.WorkPointId, x.TotalPrice, x.Status, x.Products));

            return new JsonResult(dtoList);
        }
    }
}
