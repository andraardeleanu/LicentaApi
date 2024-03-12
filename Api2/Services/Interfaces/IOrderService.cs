using Api2.ApiModels;
using Api2.Requests;

namespace Api2.Services.Interfaces
{ //not sure about structure of the project. Maybe this should be moved to core sln. But then you need some common models or to duplicate this OrderRequest ( as far I remember)
    public interface IOrderService
    {
        Task<GenericResponse<object>> AddOrderAsync(OrderRequest orderRequest);
    }
}

