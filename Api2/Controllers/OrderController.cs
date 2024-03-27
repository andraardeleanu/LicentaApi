using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Api2.Services.Interfaces;
using Core.Entities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.Common;
using Microsoft.AspNetCore.Http;


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
            var encounteredProductIds = new HashSet<int>();
            foreach (var product in orderRequest.Products)
            {
                if (encounteredProductIds.Contains(product.ProductId))
                {
                    return BadRequest($"Duplicate product ID encountered: {product.ProductId}. Product IDs must be unique.");
                }
                encounteredProductIds.Add(product.ProductId);
            }

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
            orderRequest.Author = username;
            var res = await _orderServiceTest.AddOrderAsync(orderRequest, Enums.OrderType.Manual);
            return new JsonResult(res);
        }

        [HttpPost]
        [Route("addOrdersFromFile")]
        public async Task<IActionResult> CreateOrdersFromCsvAsync([FromForm] IFormFile file, [FromForm] int workPointId)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file.");
            }
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";

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
                            CreatedBy = 0, // Remove it, until then put a valid id
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
