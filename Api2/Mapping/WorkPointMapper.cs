using Api2.Requests;
using Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api2.Mapping
{
    public static class WorkPointMapper
    {
        public static WorkPoint ToWorkpointEntity(this WorkpointRequest request, string createdBy, string user, int companyId)
        {
            return new WorkPoint()
            {
                Name = request.Name,
                Address = request.Address,
                CompanyId = companyId,
                CreatedBy = createdBy,
                Author = user,
                DateCreated = DateTime.Now,
            };
        }
    }
}
