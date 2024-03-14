using Core.Entities;

namespace Api2.ApiModels
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public Guid OrderNo { get; set; }
        public DateTime Date { get; set; }
        public int WorkPointId { get; set; }
        public string Status { get; set; }
        public IEnumerable<ProductWithQuantity> Products;
    }
}
