namespace Api2.Responses
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockId { get; set; }
        public decimal AvailableStock { get; set; }
    }
}
