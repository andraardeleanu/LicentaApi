using Core.Common;

namespace Api2.Requests
{
    public class ProductDetails
    {
        public int ProductId { get; set; }
        public int Quantity{ get; set; }
    }

    public class OrderRequest
    {
        public Guid OrderNo { get; set; }
        public string? Author { get; set; }
        public string CreatedBy { get; set; }
        public int WorkPointId { get; set; }
        public List<ProductDetails> Products { get; set; }
    }
}
