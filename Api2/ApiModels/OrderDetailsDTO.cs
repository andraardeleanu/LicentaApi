using Core.Common;
using Core.Entities;

namespace Api2.ApiModels
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public Guid OrderNo { get; set; }
        public Enums.OrderType OrderType { get; set; }
        public int WorkPointId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public IEnumerable<ProductWithQuantity> Products;
    }
}
