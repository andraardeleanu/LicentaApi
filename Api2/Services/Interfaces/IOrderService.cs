using System;
using Core.Entities;
using Api2.Requests;

namespace Core.Services.Interfaces
{ //not sure about structure of the project. Maybe this should be moved to core sln. But then you need some common models or to duplicate this OrderRequest ( as far I remember)
	public interface IOrderService
    {
        Task<bool> AddOrderAsync<OrderRequest>(OrderRequest orderRequest);
    }
}

