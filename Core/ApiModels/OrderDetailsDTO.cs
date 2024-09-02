using Core.Common;
using Core.Entities;

namespace Core.ApiModels
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public Guid OrderNo { get; set; }
        public int WorkPointId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public IEnumerable<ProductWithQuantity> Products;
    }
}
