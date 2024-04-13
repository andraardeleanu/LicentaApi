namespace Api2.Requests
{
    public class BillRequest
    {
        public string OrderNo { get; set; }
        public string? Author { get; set; }
        public string CreatedBy { get; set; }
        public int WorkPointId { get; set; }
        public List<ProductDetails> Products { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
