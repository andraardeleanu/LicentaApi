using Api2.Requests;
using Core.Entities;

namespace Api2.Mapping
{
    public static class WorkPointMapper
    {
        public static WorkPoint ToWorkpointEntity(this WorkpointRequest request, string user)
        {
            return new WorkPoint()
            {
                Name = request.Name,
                Address = request.Address,
                CompanyId = request.CompanyId,
                Author = user,
                DateCreated = DateTime.Now,
            };
        }
    }
}
