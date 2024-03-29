using Api2.Requests;
using Core.Entities;

namespace Api2.Mapping
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
