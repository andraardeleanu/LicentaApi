using System;
using Core.Entities;
namespace Api2.ApiModels
{ //maybe you should find another place for this class 
	public class ProductWithQuantity:Product
	{
		public int Quantity { get; set; }
	}
}

