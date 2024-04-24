using Api2.ApiModels;
using Api2.Requests;
using Core.Entities;

namespace Api2.Mapping
{
    public static class BillMapper
    {
        public static Bill ToBillEntity(this BillRequest request, string user)
        {
            return new Bill()
            {
                CreatedBy = user,                
                DateCreated = DateTime.Now,
                Status = request.Status,
                Products = request.Products,
                OrderNo = request.OrderNo,
                TotalPrice = request.TotalPrice,
                OrderType = request.OrderType,
                WorkPointId = request.WorkPointId,
            };
        }
    }
}
