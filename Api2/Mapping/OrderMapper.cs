﻿using Api2.ApiModels;
using Api2.Requests;
using Core.Entities;
using static Core.Common.Enums;

namespace Api2.Mapping
{
    public static class OrderMapper
    {
        public static Order ToOrderEntityCreate(this OrderRequest request,string user)
        {
            return new Order()
            {
                Date = DateTime.Now,
                Author = user,
                CreatedBy=request.CreatedBy,//delete it
                OrderNo = Guid.NewGuid(),
                FileType=request.FileType,
                Status= OrderStatus.Initialized,
                WorkPointId=request.WorkPointId
            };
        }

        public static OrderDetailsDTO ToOrderDetailsDTO(Order order, List<Product> products)
        {
            return new OrderDetailsDTO()
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                Date = order.Date,
                WorkPointId = order.WorkPointId,
                Status = order.Status,
                Products = products

            };
    }
    }
}
