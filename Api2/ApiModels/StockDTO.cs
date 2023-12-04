namespace Api2.ApiModels
{
    public class StockDTO
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string AvailableStock { get; set; }
        public string PendingStock { get; set; }
        public StockDTO(string id, string productId, string availableStock, string pendingStock)
        {
            Id = id;
            ProductId = productId;
            AvailableStock = availableStock;
            PendingStock = pendingStock;
        }
    }
}
