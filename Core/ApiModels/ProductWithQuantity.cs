using System;
using Core.Entities;
namespace Core.ApiModels
{
    public class ProductWithQuantity : Product
    {
        public int Quantity { get; set; }
    }
}

