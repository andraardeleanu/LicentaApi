namespace Api2.ApiModels
{
    public class StockDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int AvailableStock { get; set; }
        public int PendingStock { get; set; }
        public StockDTO(int id, int productId, string productName, int availableStock, int pendingStock)
        {
            Id = id;
            ProductId = productId;
            ProductName = productName;
            AvailableStock = availableStock;
            PendingStock = pendingStock;
        }
    }
}
