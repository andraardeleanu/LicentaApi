﻿using Core.ApiModels;
using Core.Entities;
using Core.Requests;

namespace Core.Mapping
{
    public static class BillMapper
    {
        public static Bill ToBillEntity(this BillRequest request, string user)
        {
            return new Bill()
            {
                Author = user,
                CreatedBy = request.CreatedBy,
                OrderNo = request.OrderNo,
                Status = request.Status,
                TotalPrice = request.TotalPrice,
                WorkpointName = request.WorkpointName,
                CompanyName = request.CompanyName,
            };
        }

        public static BillsDetailsDTO ToBillDetailsDTO(Order order, string customer, string orderWorkpoint, string workpointCompany, List<ProductWithQuantity> products)
        {
            return new BillsDetailsDTO()
            {
                OrderNo = order.OrderNo,
                Customer = customer,
                WorkpointName = orderWorkpoint,
                CompanyName = workpointCompany,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                Products = products
            };
        }
    }
}
