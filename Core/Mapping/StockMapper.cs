using Core.Entities;

namespace Core.Mapping
{
    public static class StockMapper
    {
        public static Stock ToStockMapper(int availableStock, int productId, string user)
        {
            return new Stock()
            {
                AvailableStock = availableStock,
                PendingStock = 0,
                ProductId = productId,
                Author = user,
                DateCreated = DateTime.Now,
            };
        }

    }
}
