using Core.Entities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Common;
using Microsoft.AspNetCore.Identity;
using Infra.Data.Auth;
using Core.Constants;
using Microsoft.IdentityModel.Tokens;
using Core.Models;
using Core.Requests;
using Core.Responses;
using Core.Mapping;
using Core.ApiModels;

namespace Api2.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderProduct> _orderProductService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOrderService _orderServiceTest;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericService<WorkPoint> _workpointService;

        public OrderController(IGenericService<Order> orderService,
            IGenericService<OrderProduct> orderProductService,
            RoleManager<IdentityRole> roleManager,
            IOrderService orderServiceTest,
            UserManager<ApplicationUser> userManager,
            IGenericService<WorkPoint> workpointService)
        {
            _orderService = orderService;
            _roleManager = roleManager;
            _orderProductService = orderProductService;
            _orderServiceTest = orderServiceTest;
            _userManager = userManager;
            _workpointService = workpointService;
        }

        [HttpGet]
        [Authorize]
        [Route("getOrders")]
        public async Task<IActionResult> GetOrderAsync([FromQuery] OrderFilterRequest orderRequest)
        {
            var orders = await _orderService.ListAsync();
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username!);
            var role = (await _userManager.GetRolesAsync(user!)).FirstOrDefault();

            if (role == "Customer")
            {
                orders = orders.FindAll(x => x.CreatedBy == user!.Id);
            }

            if (orderRequest.Id != null)
            {
                orders = orders.FindAll(x => x.Id == orderRequest.Id);
            }

            if (orderRequest.OrderNo != null)
            {
                orders = orders.FindAll(order => order.OrderNo.ToString().Contains(orderRequest.OrderNo.ToString()!));
            }

            if (orderRequest.WorkPointId != null)
            {
                orders = orders.FindAll(x => x.WorkPointId == orderRequest.WorkPointId);
            }

            if (orderRequest.Status != null)
            {
                orders = orders.FindAll(order => order.Status.ToString().Contains(orderRequest.Status.ToString()!));
            }

            var dtoList = orders.Select(x => new OrderDTO(x.Id, x.OrderNo, x.Author, x.DateCreated, x.WorkPointId, x.TotalPrice, x.Status));

            dtoList = dtoList.OrderByDescending(x => x.DateCreated).ToList();

            return new JsonResult(dtoList);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("updateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatusAsync([FromBody] UpdateOrderStatusRequest request)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(request.Id);

                if (order.Status == Enums.OrderStatus.Procesata.ToString()) return BadRequest(new Result(ErrorMessages.OrderStatusError));

                order.Status = Enums.OrderStatus.Procesata.ToString();
                order.DateUpdated = DateTime.UtcNow;

                await _orderService.UpdateAsync(order);

                return Ok(new Result());

            }
            catch (Exception)
            {
                return BadRequest(new Result(ErrorMessages.InvalidData));
            }
        }

        [HttpGet]
        [Authorize]
        [Route("getOrderDetails")]
        public async Task<IActionResult> GetOrderDetailsAsync([FromQuery] int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId, x => x.OrderProduct);
            var orderProducts = await _orderProductService.WhereAsync(x => x.OrderId == orderId, y => y.Product);

            var productsWithQuantity = orderProducts.Select(op => new ProductWithQuantity
            {
                Name = op.Product.Name,
                Price = op.Product.Price,
                Quantity = op.Quantity
            }).ToList();

            var orderDetail = OrderMapper.ToOrderDetailsDTO(order, productsWithQuantity);

            return new JsonResult(orderDetail.Products.ToList());
        }

        [HttpGet]
        [Authorize]
        [Route("getOrderDetailsForBill")]
        public async Task<IActionResult> GetOrderDetailsForBillAsync([FromQuery] int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId, x => x.OrderProduct);
            var orderProducts = await _orderProductService.WhereAsync(x => x.OrderId == orderId, y => y.Product);

            var productsWithQuantity = orderProducts.Select(op => new ProductWithQuantity
            {
                Name = op.Product.Name,
                Price = op.Product.Price,
                Quantity = op.Quantity
            }).ToList();

            var orderDetail = OrderMapper.ToOrderDetailsDTO(order, productsWithQuantity);

            return new JsonResult(orderDetail);
        }

        [HttpPost]
        [Authorize]
        [Route("addOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest orderRequest)
        {
            try
            {
                var existingWorkpoint = await _workpointService.GetByIdAsync(orderRequest.WorkPointId);
            }
            catch (Exception)
            {
                return BadRequest(new Result(ErrorMessages.InvalidWorkpoint));
            }

            if (orderRequest.Products.IsNullOrEmpty())
            {
                return BadRequest(new Result(ErrorMessages.EmptyProductList));
            }
            else
            {
                var encounteredProductIds = new HashSet<int>();
                foreach (var product in orderRequest.Products)
                {
                    if (encounteredProductIds.Contains(product.ProductId))
                    {
                        return BadRequest(new Result(ErrorMessages.DuplicatedProduct));
                    }

                    else if (product.Quantity < 0)
                    {
                        return BadRequest(new Result(ErrorMessages.InvalidQuantity));
                    }

                    encounteredProductIds.Add(product.ProductId);
                }

                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByNameAsync(username!);

                orderRequest.Author = username;
                orderRequest.CreatedBy = user!.Id;

                var res = await _orderServiceTest.AddOrderAsync(orderRequest, Enums.OrderType.Manual);

                if (res.StatusCode != 500)
                {
                    var data = (dynamic)res.Data;
                    var orderTotalPrice = data.TotalPrice;
                    var orderId = data.OrderId;
                    var orderEntity = await _orderService.GetByIdAsync(orderId);
                    orderEntity.TotalPrice = orderTotalPrice;

                    await _orderService.UpdateAsync(orderEntity);

                    var orderResponse = new OrderResponse { Id= orderEntity.Id, OrderNo = orderEntity.OrderNo, WorkpointId = orderRequest.WorkPointId, TotalPrice = orderTotalPrice };

                    return Ok(new Result<OrderResponse>(orderResponse));
                }
                else
                {
                    return BadRequest(new Result(res.Data.ToString()!));
                }
            }
        }

        [HttpPost]
        [Authorize]
        [Route("addOrdersFromFile")]
        public async Task<IActionResult> CreateOrdersFromCsvAsync(IFormFile file, [FromForm] int workPointId)
        {
            try
            {
                var existingWorkpoint = await _workpointService.GetByIdAsync(workPointId);
            }
            catch (Exception)            
            {
                return BadRequest(new Result(ErrorMessages.InvalidWorkpoint));
            }

            if (file == null || file.Length <= 0)
                return BadRequest(new Result(ErrorMessages.InvalidFile));

            var ext = Path.GetExtension(file.FileName);
            if (ext != ".csv")
                return BadRequest(new Result(ErrorMessages.InvalidFileExtension));

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var products = new List<ProductDetails>();
                    var encounteredProductIds = new HashSet<int>();
                    string line;
                    while ((line = reader.ReadLine()!) != null)
                    {
                        var values = line.Split(',');
                        if (values.Length >= 2)
                        {
                            if (int.TryParse(values[0], out int productId) && int.TryParse(values[1], out int quantity))
                            {
                                if (encounteredProductIds.Contains(productId))
                                {
                                    return BadRequest(new Result(ErrorMessages.DuplicatedProduct));
                                }

                                products.Add(new ProductDetails
                                {
                                    ProductId = productId,
                                    Quantity = quantity
                                });
                                encounteredProductIds.Add(productId);
                            }
                        }
                        else
                            return BadRequest(new Result(ErrorMessages.FileFormatError));
                    }

                    var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
                    var user = await _userManager.FindByNameAsync(username);

                    if (products.Count > 0)
                    {
                        var order = new OrderRequest
                        {
                            Author = username,
                            CreatedBy = user!.Id,
                            WorkPointId = workPointId,
                            Products = products
                        };

                        var orderResponse = await _orderServiceTest.AddOrderAsync(order, Enums.OrderType.File);

                        if (orderResponse.StatusCode == 500)
                        {
                            var orderRespMessage = orderResponse.Data.ToString();
                            return BadRequest(new Result(orderRespMessage!));
                        }
                        else
                        {
                            var data = (dynamic)orderResponse.Data;
                            var orderTotalPrice = data.TotalPrice;
                            var orderId = data.OrderId;
                            var orderEntity = await _orderService.GetByIdAsync(orderId);
                            orderEntity.TotalPrice = orderTotalPrice;

                            await _orderService.UpdateAsync(orderEntity);

                            var orderResp = new OrderResponse { Id = orderEntity.Id, OrderNo = order.OrderNo, WorkpointId = order.WorkPointId, TotalPrice = orderTotalPrice };

                            return Ok(new Result<OrderResponse>(orderResp));
                        }
                    }
                    else
                        return BadRequest(new Result(ErrorMessages.EmptyProductList));
                }
            }
            catch (Exception)
            {
                return BadRequest(new Result(ErrorMessages.InvalidData));
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("removeOrder")]
        public async Task<IActionResult> RemoveOrderAsync(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            await _orderService.DeleteAsync(order);

            var orderResponse = new OrderResponse { Id = order.Id };
            return Ok(new Result<OrderResponse>(orderResponse));
        }
    }
}
