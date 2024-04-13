using Api2.ApiModels;
using Api2.Requests;
using Core.Entities;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api2.Mapping
{
    public static class BillMapper
    {
        public static Bill ToBillEntity(this BillRequest request, string user)
        {
            return new Bill()
            {
                Status = request.Status,
                OrderNo = request.OrderNo,
                CreatedBy = request.CreatedBy,
                Author = user,
                TotalPrice = request.TotalPrice,
                DateCreated = DateTime.Now,
            };
        }

        public static BillsDTO ToBillDTOEntity(this BillRequest request)
        {
            return new BillsDTO(request.OrderNo, DateTime.Now, request.CreatedBy, request.WorkPointId, request.TotalPrice, request.Status, request.Products);

        }
    }
}
