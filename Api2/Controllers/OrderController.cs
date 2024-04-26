using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Api2.Services.Interfaces;
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
using System.Text.Json;
using LanguageExt.ClassInstances;

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
        //[Authorize]
        [Route("getOrders")]
        public async Task<IActionResult> GetOrderAsync([FromQuery] OrderFilterRequest orderRequest)
        {
            var orders = await _orderService.ListAsync();
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if (role == "Customer")
            {
                orders = orders.FindAll(x => x.CreatedBy == user.Id);
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

            var dtoList = orders.Select(x => new OrderDTO(x.Id, x.OrderNo, x.Date, x.WorkPointId, x.Status));

            return new JsonResult(dtoList);
        }

        [HttpGet]
        [Authorize]
        [Route("getOrdersByUserId/{id}")]
        public async Task<IActionResult> GetOrdersFromUser([FromRoute] string id)
        {
            var userOrders = await _orderService.WhereAsync(x => x.CreatedBy == id);

            return Ok(userOrders);
        }

        [HttpPost]
        //[Authorize]
        [Route("updateOrderStatus/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(orderId);

                if (order.Status == Enums.OrderStatus.Processed.ToString()) return BadRequest(new Result(ErrorMessages.OrderStatusError));

                order.Status = Enums.OrderStatus.Processed.ToString();
                order.DateUpdated = DateTime.UtcNow;

                await _orderService.UpdateAsync(order);

                return Ok(new Result());

            }
            catch (Exception ex)
            {
                return BadRequest(new Result(ErrorMessages.InvalidData));
            }
        }

        [HttpGet]
        [Route("getOrderDetails/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsAsync(int orderId)
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
        [Route("getOrderDetailsForBill/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsForBillAsync(int orderId)
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
        // [Authorize]
        [Route("addOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest orderRequest)
        {
            try
            {
                var existingWorkpoint = await _workpointService.GetByIdAsync(orderRequest.WorkPointId);
            }
            catch (Exception ex)
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
                var user = await _userManager.FindByNameAsync(username);

                orderRequest.Author = username;
                orderRequest.CreatedBy = user.Id;

                var res = await _orderServiceTest.AddOrderAsync(orderRequest, Enums.OrderType.Manual);

                if (res.StatusCode != 500)
                {
                    var data = (dynamic)res.Data;
                    var orderTotalPrice = data.TotalPrice;
                    var orderId = data.OrderId;
                    var orderEntity = await _orderService.GetByIdAsync(orderId);
                    orderEntity.TotalPrice = orderTotalPrice;

                    await _orderService.UpdateAsync(orderEntity);

                    return Ok(new Result());
                }
                else
                {
                    return BadRequest(new Result(res.Data.ToString()));
                }
            }
        }

        [HttpPost]
        [Route("addOrdersFromFile")]
        public async Task<IActionResult> CreateOrdersFromCsvAsync(IFormFile file, [FromForm] int workPointId)
        {
            try
            {
                var existingWorkpoint = await _workpointService.GetByIdAsync(workPointId);
            }
            catch (Exception ex)
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
                    while ((line = reader.ReadLine()) != null)
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
                            OrderNo = Guid.NewGuid(),
                            Author = username,
                            CreatedBy = user.Id,
                            WorkPointId = workPointId,
                            Products = products
                        };

                        var orderResponse = await _orderServiceTest.AddOrderAsync(order, Enums.OrderType.File);

                        if (orderResponse.StatusCode == 500)
                        {
                            var orderRespMessage = orderResponse.Data.ToString();
                            return BadRequest(new Result(orderRespMessage));
                        }
                        else
                        {
                            var data = (dynamic)orderResponse.Data;
                            var orderTotalPrice = data.TotalPrice;
                            var orderId = data.OrderId;
                            var orderEntity = await _orderService.GetByIdAsync(orderId);
                            orderEntity.TotalPrice = orderTotalPrice;

                            await _orderService.UpdateAsync(orderEntity);
                            return Ok(new Result());
                        }
                    }
                    else
                        return BadRequest(new Result(ErrorMessages.EmptyProductList));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new Result(ErrorMessages.InvalidData));
            }
        }
    }
}
