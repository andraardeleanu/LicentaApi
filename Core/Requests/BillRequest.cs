using Core.Entities;

namespace Core.Requests
{
    public class BillRequest
    {
        public string CreatedBy { get; set; }
        public Guid OrderNo { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string WorkpointName { get; set; }
        public string CompanyName { get; set; }
        public IEnumerable<OrderProduct> Products { get; set; }
    }
}
