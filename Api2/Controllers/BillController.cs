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
using System.Linq;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class BillController : ControllerBase
    {
        private readonly IGenericService<Bill> _billService;
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderProduct> _orderProductService;
        private readonly IGenericService<WorkPoint> _workpointService;
        private readonly IGenericService<Company> _companyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillController(IGenericService<Order> orderService,
            IGenericService<Bill> billService,
            IGenericService<OrderProduct> orderProductService,
            IGenericService<WorkPoint> workpointService,
            IGenericService<Company> companyService,
            UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _billService = billService;
            _orderProductService = orderProductService;
            _workpointService = workpointService;
            _companyService = companyService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("billGenerator")]
        public async Task<IActionResult> GenerateOrderBill([FromBody] BillRequest request)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);

            var billedOrder = (await _orderService.WhereAsync(x => x.OrderNo == request.OrderNo)).FirstOrDefault();
            var orderWorkpoint = await _workpointService.GetByIdAsync(billedOrder.WorkPointId);
            var workpointCompany = (await _companyService.WhereAsync(x => x.Id == orderWorkpoint.CompanyId)).FirstOrDefault();

            if (request.Status == Enums.OrderStatus.Initializata.ToString())
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
                request.CreatedBy = user.Id;
                request.WorkpointName = orderWorkpoint.Name;
                request.CompanyName = workpointCompany.Name;

                var billEntity = request.ToBillEntity(username);
                await _billService.AddAsync(billEntity);

                billedOrder.Status = Enums.OrderStatus.Facturata.ToString();
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

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if (role == "Customer")
            {
                bills = bills.FindAll(x => x.CreatedBy == user.Id);
            }

            bills = bills.OrderByDescending(x => x.DateCreated).ToList();

            var dtoList = bills.Select(x => new BillsDTO(x.Author, x.DateCreated, x.OrderNo, x.WorkpointName, x.CompanyName, x.TotalPrice, x.Status));

            return new JsonResult(dtoList);
        }

        [HttpGet]
        //[Authorize]
        [Route("getBillDetails/{orderId}")]
        public async Task<IActionResult> GetBillDetailsAsync(int orderId)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var order = await _orderService.GetByIdAsync(orderId);
            var orderWorkpoint = await _workpointService.GetByIdAsync(order.WorkPointId);
            var workpointCompany = (await _companyService.WhereAsync(x => x.Id == orderWorkpoint.CompanyId)).FirstOrDefault();
            var orderProducts = await _orderProductService.WhereAsync(x => x.OrderId == orderId, y => y.Product);

            List<ProductWithQuantity> productsWithQuantity = new List<ProductWithQuantity>();
            productsWithQuantity = orderProducts.Select(op => new ProductWithQuantity
            {
                Name = op.Product.Name,
                Price = op.Product.Price,
                Quantity = op.Quantity
            }).ToList();

            var billDetails = BillMapper.ToBillDetailsDTO(order, orderWorkpoint.Name, workpointCompany.Name, productsWithQuantity);


            return new JsonResult(billDetails);
        }
    }
}
