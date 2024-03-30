using Api2.Requests;
using Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api2.Mapping
{
    public static class WorkPointMapper
    {
        public static WorkPoint ToWorkpointEntity(this WorkpointRequest request, string createdBy, string user)
        {
            return new WorkPoint()
            {
                Name = request.Name,
                Address = request.Address,
                CompanyId = request.CompanyId,
                CreatedBy = createdBy,
                Author = user,
                DateCreated = DateTime.Now,
            };
        }
    }
}
