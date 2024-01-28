using Api2.ApiModels;
using Api2.Requests;
using Core.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api2.Mapping
{
    public static class OrderMapper
    {
        public static Order ToOrderEntity(string user)
        {
            return new Order()
            {
                Date = DateTime.Now,
                Author = user,
            };
        }

        public static OrderDetailsDTO ToOrderDetailsDTO(Order order, List<Product> products)
        {
            return new OrderDetailsDTO()
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                Date = order.Date,
                WorkPointId = order.WorkPointId,
                Status = order.Status,
                Products = products

            };
    }
    }
}
