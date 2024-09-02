namespace Core.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int AvailableStock { get; set; }
    }
}
