using Core.Entities;

namespace Api2.ApiModels
{
    public class BillsDetailsDTO
    {
        public Guid OrderNo { get; set; }
        public string WorkpointName { get; set; }
        public string CompanyName { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public List<ProductWithQuantity> Products { get; set; }
    }
}
