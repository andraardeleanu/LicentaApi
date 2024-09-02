using Core.ApiModels;
using Core.Common;
using Core.Constants;
using Core.Entities;
using Core.Mapping;
using Core.Models;
using Core.Requests;
using Core.Responses;
using Core.Services.Interfaces;
using Infra.Data.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GenerateOrderBill([FromBody] BillGeneratorRequest request)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username!);
            var billedOrder = (await _orderService.WhereAsync(x => x.OrderNo.ToString() == request.OrderNo.ToUpper())).FirstOrDefault();

            if (billedOrder == null)
            {
                return BadRequest(new Result(ErrorMessages.InvalidData));
            }

            var orderWorkpoint = await _workpointService.GetByIdAsync(billedOrder!.WorkPointId);
            var workpointCompany = (await _companyService.WhereAsync(x => x.Id == orderWorkpoint.CompanyId)).FirstOrDefault();

            if (billedOrder!.Status == Enums.OrderStatus.Initializata.ToString())
            {
                return BadRequest(new Result(ErrorMessages.OrderStatusBillError));
            }

            var existingBill = await _billService.WhereAsync(x => x.OrderNo.ToString() == request.OrderNo);
            if (existingBill != null && existingBill.Any())
            {
                return BadRequest(new Result(ErrorMessages.BillInsertionError));
            }

            BillRequest currentBill = new();

            currentBill.OrderNo = billedOrder.OrderNo;
            currentBill.TotalPrice = billedOrder.TotalPrice;
            currentBill.CreatedBy = user!.Id;
            currentBill.WorkpointName = orderWorkpoint.Name;
            currentBill.CompanyName = workpointCompany!.Name;
            currentBill.Status = billedOrder.Status;
            currentBill.Products = billedOrder.OrderProduct;

            var billEntity = currentBill.ToBillEntity(username!);
            await _billService.AddAsync(billEntity);

            billedOrder.Status = Enums.OrderStatus.Facturata.ToString();
            billedOrder.DateUpdated = DateTime.UtcNow;

            await _orderService.UpdateAsync(billedOrder);

            var billResponse = new BillResponse { BillId = billEntity.Id };
            return Ok(new Result<BillResponse>(billResponse));
        }

        [HttpGet]
        [Authorize]
        [Route("getBills")]
        public async Task<IActionResult> GetBillsAsync()
        {
            var bills = await _billService.ListAsync();

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username!);
            var role = (await _userManager.GetRolesAsync(user!)).FirstOrDefault();

            if (role == "Customer")
            {
                bills = bills.FindAll(x => x.CreatedBy == user!.Id);
            }

            bills = bills.OrderByDescending(x => x.DateCreated).ToList();

            var dtoList = bills.Select(x => new BillsDTO(x.Id, x.Author, x.DateCreated, x.OrderNo, x.WorkpointName, x.CompanyName, x.TotalPrice, x.Status));

            return new JsonResult(dtoList);
        }

        [HttpGet]
        [Authorize]
        [Route("getBillDetails")]
        public async Task<IActionResult> GetBillDetailsAsync([FromQuery] int orderId)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var order = await _orderService.GetByIdAsync(orderId);
            var orderWorkpoint = await _workpointService.GetByIdAsync(order.WorkPointId);
            var orderCustomer = await _userManager.FindByIdAsync(order.CreatedBy);
            var workpointCompany = (await _companyService.WhereAsync(x => x.Id == orderWorkpoint.CompanyId)).FirstOrDefault();
            var orderProducts = await _orderProductService.WhereAsync(x => x.OrderId == orderId, y => y.Product);

            List<ProductWithQuantity> productsWithQuantity = new List<ProductWithQuantity>();
            productsWithQuantity = orderProducts.Select(op => new ProductWithQuantity
            {
                Name = op.Product.Name,
                Price = op.Product.Price,
                Quantity = op.Quantity
            }).ToList();

            var billDetails = BillMapper.ToBillDetailsDTO(order, orderCustomer!.UserName!, orderWorkpoint.Name, workpointCompany!.Name, productsWithQuantity);

            return new JsonResult(billDetails);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("removeBill")]
        public async Task<IActionResult> RemoveBillAsync(int id)
        {
            var bill = await _billService.GetByIdAsync(id);
            await _billService.DeleteAsync(bill);

            var billResponse = new BillResponse { BillId = bill.Id };
            return Ok(new Result<BillResponse>(billResponse));
        }
    }
}
