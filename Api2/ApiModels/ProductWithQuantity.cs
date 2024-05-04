using System;
using Core.Entities;
namespace Api2.ApiModels
{
    public class ProductWithQuantity : Product
    {
        public int Quantity { get; set; }
    }
}

