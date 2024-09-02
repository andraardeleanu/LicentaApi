using Core.ApiModels;
using Core.Entities;
using Core.Requests;
using static Core.Common.Enums;

namespace Core.Mapping
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
                WorkPointId = order.WorkPointId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                Products = products
            };
        }
    }
}
