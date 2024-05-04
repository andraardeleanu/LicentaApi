using Api2.ApiModels;
using Api2.Requests;
using Core.Entities;
using static Core.Common.Enums;

namespace Api2.Mapping
{
    public static class OrderMapper
    {
        public static Order ToOrderEntityCreate(this OrderRequest request, string user)
        {
            return new Order()
            {
                Author = user,
                CreatedBy = request.CreatedBy,
                OrderNo = Guid.NewGuid(),
                Status = OrderStatus.Initializata.ToString(),
                WorkPointId = request.WorkPointId,
                TotalPrice = request.TotalPrice
            };
        }

        public static OrderDetailsDTO ToOrderDetailsDTO(Order order, List<ProductWithQuantity> products)
        {
            return new OrderDetailsDTO()
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                OrderType = order.OrderType,
                WorkPointId = order.WorkPointId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                Products = products
            };
        }
    }
}
