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

namespace Api2.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderProduct> _orderProductService;
        private readonly IOrderService _orderServiceTest;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IGenericService<Order> orderService, IGenericService<OrderProduct> orderProductService, IOrderService orderServiceTest, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _orderProductService = orderProductService;
            _orderServiceTest = orderServiceTest;
            _userManager = userManager;
        }

        [HttpGet]
        //[Authorize]
        [Route("getOrders")]
        public async Task<IActionResult> GetOrderAsync([FromQuery] OrderFilterRequest orderRequest)
        {
            var orders = await _orderService.ListAsync();

            if(orderRequest.OrderNo != null)
            {
                orders = orders.FindAll(order => order.OrderNo.ToString().Contains(orderRequest.OrderNo.ToString()!));
            }

            if (orderRequest.WorkPointId != null)
            {
                orders = orders.FindAll(x => x.WorkPointId == orderRequest.WorkPointId);
            }

            if (orderRequest.Status != null)
            {
                orders = orders.FindAll(x => x.Status == orderRequest.Status);
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

        [HttpPost]
        // [Authorize]
        [Route("addOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest orderRequest)
        {
            var encounteredProductIds = new HashSet<int>();
            foreach (var product in orderRequest.Products)
            {
                if (encounteredProductIds.Contains(product.ProductId))
                {
                    return BadRequest(ErrorMessages.DuplicatedProduct + product.ProductId);
                }
                encounteredProductIds.Add(product.ProductId);
            }

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);

            orderRequest.Author = username;
            orderRequest.CreatedBy = user.Id;
            var res = await _orderServiceTest.AddOrderAsync(orderRequest, Enums.OrderType.Manual);
            return new JsonResult(res);
        }

        [HttpPost]
        [Route("addOrdersFromFile")]
        public async Task<IActionResult> CreateOrdersFromCsvAsync(IFormFile file, [FromForm] int workPointId)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file.");
            }
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
            var user = await _userManager.FindByNameAsync(username);

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
                                    return BadRequest($"Duplicate product ID encountered: {productId}. Product IDs must be unique.");
                                }

                                products.Add(new ProductDetails
                                {
                                    ProductId = productId,
                                    Quantity = quantity
                                });

                                encounteredProductIds.Add(productId);
                            }
                        }
                    }

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

                        await _orderServiceTest.AddOrderAsync(order, Enums.OrderType.File);

                        return Ok("Order added successfully from CSV.");
                    }
                    else
                    {
                        return BadRequest("No valid products found in the CSV file.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
