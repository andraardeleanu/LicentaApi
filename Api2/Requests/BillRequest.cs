using Core.Common;
using Core.Entities;

namespace Api2.Requests
{
    public class BillRequest
    {
        public string OrderNo { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public Enums.OrderType OrderType { get; set; } // change this prop to order type in db and models
        public int WorkPointId { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
