using Core.Common;
using Core.Entities;

namespace Api2.Requests
{
    public class BillRequest
    {
        public string CreatedBy { get; set; }
        public Guid OrderNo { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string WorkpointName { get; set; }
        public string CompanyName { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
