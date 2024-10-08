﻿using Core.Common;

namespace Core.Requests
{
    public class ProductDetails
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderRequest
    {
        public Guid OrderNo { get; set; }
        public string? Author { get; set; }
        public string? CreatedBy { get; set; }
        public int WorkPointId { get; set; }
        public decimal TotalPrice { get; set; }
        public required List<ProductDetails> Products { get; set; }
    }
}
