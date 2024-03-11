using Core.Common;

namespace Api2.Requests
{
    public class ProductDetails //move it from here if you want
    {
        public int ProductId { get; set; }
        public int Quantity{ get; set; }
    }

    public class OrderRequest
    {
        public Guid OrderNo { get; set; }
        public int CreatedBy { get; set; } // to be deleted also from DB
        public int WorkPointId { get; set; }
        public Enums.OrderType FileType { get; set; }
        public IEnumerable<ProductDetails> Products { get; set; }
    }
}
